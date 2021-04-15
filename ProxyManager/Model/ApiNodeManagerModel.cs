using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyManager.Model
{
    public class ApiNodeManagerModel
    {
        public class NodeInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string location { get; set; }
            public string ip { get; set; }
            public int port { get; set; }
            public DateTime lastHBDate { get; set; }
            public string state { get; set; }
            public long use_count { get; set; }
        }
    }
}
