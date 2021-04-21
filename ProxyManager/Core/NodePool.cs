using Microsoft.Extensions.Configuration;
using ProxyManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyManager.Core
{
    public class NodePool
    {
        public List<NodeInfo> Nodes;
        private HttpClient Http;
        private Thread MainThread;
        private object locker;
        private int RedialInterval = 99999;
        private bool EnableAutoRedial = false;
        private DateTime NextCheckDate = DateTime.Now;
        private IConfiguration Config;
        private NodePool(IConfiguration _config)
        {
            this.Config = _config;
            Nodes = new List<NodeInfo>();
            Http = new HttpClient();
            MainThread = new Thread(new ThreadStart(Work));
            locker = new object();
            this.UpdateRedialInterval();
            MainThread.Start();
        }
        public NodeInfo AddOrUpdate(ApiNodeManagerModel.NodeInfo info)
        {
            lock (locker)
            {
                if (Nodes.Any(i => i.id == info.id))
                {
                    var _node = Nodes.Where(i => i.id == info.id).First();
                    if (_node.actionTask.Count == 0)
                    {
                        _node.state = info.state;
                    }
                    _node.ip = info.ip;
                    _node.location = info.location;
                    _node.name = info.name;
                    _node.port = info.port;
                    _node.lastHBDate = DateTime.Now;
                    return _node;
                }
                else
                {
                    var _node = new NodeInfo();
                    _node.ip = info.ip;
                    _node.location = info.location;
                    _node.name = info.name;
                    _node.port = info.port;
                    _node.state = info.state;
                    _node.id = info.id;
                    _node.lastHBDate = DateTime.Now;
                    Nodes.Add(_node);
                    return _node;
                }
            }
        }
        public bool ReDialNode(string id)
        {
            lock (locker)
            {
                var node = Nodes.Where(i => i.id == id).FirstOrDefault();
                if (node == null)
                {
                    return false;
                }
                if (node.actionTask.Count > 0)
                {
                    return false;
                }
                node.lastRedialDate = DateTime.Now;
                node.state = "redial";
                node.actionTask.Enqueue("redial");
                return true;
            }

        }
        public bool ReDialAll()
        {
            lock (locker)
            {
                var now = DateTime.Now;
                foreach (var node in this.Nodes)
                {
                    this.ReDialNode(node.id);
                }
            }
            return true;
        }
        public NodeInfo GetOne()
        {
            NodeInfo result = null;
            lock (locker)
            {
                result = this.Nodes.Where(i=>i.state=="running").OrderBy(i => i.use_count).FirstOrDefault();
                if (result!=null)
                {
                    result.use_count += 1;
                }
            }
            return result;
        }
        public List<NodeInfo> GetAll()
        {
            List<NodeInfo> result = new List<NodeInfo>();
            lock (locker)
            {
                foreach(var node in this.Nodes)
                {
                    if (node.state == "running")
                    {
                        node.use_count += 1;
                        result.Add(node);
                    }
                }
            }
            return result;
        }
        public void UpdateRedialInterval()
        {
            RedialInterval = this.Config.GetSection("MainConfig").GetValue<int>("RedialInterval");
            EnableAutoRedial = this.Config.GetSection("MainConfig").GetValue<bool>("AutoRedial");
        }
        private void Work()
        {
            while (true)
            {
                var now = DateTime.Now;
                if (EnableAutoRedial)
                {
                    lock (locker)
                    {
                        foreach (var node in this.Nodes)
                        {
                            if((now- node.lastRedialDate).TotalSeconds >= this.RedialInterval)
                            {
                                this.ReDialNode(node.id);
                            }
                        }
                    }
                }
                if (now > NextCheckDate)
                {
                    lock (locker)
                    {
                        var interval = this.Config.GetSection("MainConfig").GetValue<int>("HeartInterval");
                        foreach (var node in this.Nodes)
                        {
                            if ((now - node.lastHBDate).TotalSeconds > interval * 3)
                            {
                                node.state = "offline";
                            }
                        }
                        NextCheckDate = now.AddSeconds(60);
                    }
                }
                Thread.Sleep(500);
            }
        }

        static NodePool np = null;
        public static NodePool Create(IConfiguration _config)
        {
            np = new NodePool(_config);
            return np;
        }
        public static NodePool Get()
        {
            return np;
        }
    }
}
