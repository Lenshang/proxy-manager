using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyManager.Model
{
    public class NodeInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string location { get; set; }
        public string state { get; set; }
        public DateTime lastHBDate { get; set; }
        public DateTime lastRedialDate { get; set; } = DateTime.Now;
        public string api_ip { get; set; }
        public int api_port { get; set; }
        public long use_count { get; set; }
        public Queue<string> actionTask { get; set; } = new Queue<string>();
        public ApiNodeManagerModel.NodeInfo ToApiNodeInfo()
        {
            return new ApiNodeManagerModel.NodeInfo()
            {
                id = this.id,
                name = this.name,
                location = this.location,
                ip = this.ip,
                port = this.port,
                lastHBDate = this.lastHBDate,
                state = this.state,
                use_count=this.use_count,
                lastRedialDate=this.lastRedialDate
            };
        }
    }
}
