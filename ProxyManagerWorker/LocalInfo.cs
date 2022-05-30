using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProxyManagerWorker
{
    public class LocalInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string location { get; set; }
        public string state { get; set; } = "init";
        public string masterApi { get; set; }

        public DateTime NextSendDate { get; set; }
        private HttpClient Http { get; set; }
        private ILogger Logger { get; set; }
        private IConfiguration Config { get; set; }
        private Thread LoopWorkThread { get; set; }
        private LocalInfo(IConfiguration configuration, ILogger logger)
        {
            this.name = configuration.GetValue<string>("NodeName");
            this.port = configuration.GetValue<int>("ProxyPort");
            this.masterApi = configuration.GetValue<string>("Master");
            this.Logger = logger;
            this.Config = configuration;
            //HttpClientHandler handler = new HttpClientHandler();
            //handler.Proxy = new WebProxy("http://127.0.0.1:8888");
            this.Http = new HttpClient();
            //this.Http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36");
            //this.Http.DefaultRequestHeaders.Add("Accept", "*/*");
            this.Http.Timeout = TimeSpan.FromSeconds(10000);
            //this.RestartService();
            this.TryGetIp();
            this.id = GetMd5(name);
            this.Logger.LogInformation("Try Connect Master:"+this.masterApi);
            while (true)
            {
                if (this.Send())
                {
                    this.Logger.LogInformation($"Connect Master Success!");
                    break;
                }
                this.Logger.LogInformation($"Connect Master {this.masterApi} Failure!");
                Thread.Sleep(1000);
            }

            this.LoopWorkThread = new Thread(new ThreadStart(this.LoopWork));
        }
        public void StartLoop()
        {
            this.state = "running";
            this.LoopWorkThread.Start();
        }
        private void LoopWork()
        {
            while (true)
            {
                if (DateTime.Now >= this.NextSendDate)
                {
                    this.Logger.LogInformation("HeartBeat");
                    this.Send();
                }
                Thread.Sleep(500);
            }
        }
        private void TryGetIp()
        {
            this.Logger.LogInformation("Try Get IpAddress");
            var t_response = this.Http.GetStringAsync(new Uri(new Uri(this.masterApi), "/api/server/ip").ToString());
            if (!t_response.Wait(10000))
            {
                throw new Exception("Get IPADDRESS FAILURE!");
            }
            var response = t_response.Result;
            //var obj = JsonConvert.DeserializeObject<JToken>(response);
            this.ip = response;
            this.location = "unknown";
            this.Logger.LogInformation($"Get IP:{this.ip} Location:{this.location}");
        }
        private void RestartService()
        {
            string cmd = this.Config.GetValue<string>("RestartCommand");
            var msg = RunnerModule.RunProcessAndGetMessage(cmd, timeout: 10000);
            this.Logger.LogInformation("Restart TinyProxy:" + msg);
        }
        private void RedialNode()
        {
            while (true)
            {
                try
                {
                    this.Logger.LogInformation("Try Redial......");
                    string cmd = this.Config.GetValue<string>("RedialCommand");
                    var msg = RunnerModule.RunProcessAndGetMessage(cmd, timeout: 10000);
                    this.Logger.LogInformation("Redial VPS:" + msg);
                    this.Http = new HttpClient();
                    this.Http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.114 Safari/537.36");
                    this.Http.DefaultRequestHeaders.Add("Accept", "*/*");
                    this.TryGetIp();
                    break;
                }
                catch
                {
                    this.Logger.LogWarning("Redial Failure,Retrying...");
                    Thread.Sleep(5000);
                }
            }

        }
        private bool Send()
        {
            try
            {
                string url = new Uri(new Uri(this.masterApi), "/api/server/receive").ToString();
                object _param = new {
                    id=this.id,
                    name=this.name,
                    location=this.location,
                    ip=this.ip,
                    port=this.port,
                    state=this.state,
                    auth= this.Config.GetValue<string>("MasterAuth")
                };
                JsonContent content = JsonContent.Create(_param);
                var t_response=this.Http.PostAsync(url, content);
                t_response.Wait(10000);
                var t_str = t_response.Result.Content.ReadAsStringAsync();
                t_str.Wait();
                var response = t_str.Result;
                var obj = JsonConvert.DeserializeObject<JToken>(response);
                this.NextSendDate = obj["data"]["next"].ToObject<DateTime>();
                //Action TASK
                string action = obj["data"]["action"].ToString();
                if (!string.IsNullOrEmpty(action))
                {
                    this.DoAction(action);
                }
                return obj["success"].ToObject<bool>();
            }
            catch(Exception e)
            {
                this.Logger.LogError(e.StackTrace);
                return false;
            }
        }
        private void DoAction(string actName)
        {
            if (actName == "redial")
            {
                this.RedialNode();
            }
            else if (actName == "get_config")
            {

            }
        }
        private string GetMd5(string userPwd)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(userPwd));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }

        static LocalInfo _l = null;
        public static LocalInfo Get()
        {
            return _l;
        }
        public static LocalInfo Create(IConfiguration configuration, ILogger logger)
        {
            _l = new LocalInfo(configuration,logger);
            return _l;
        }
    }
}
