using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProxyManager.Core;
using ProxyManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ServerController : ControllerBase
    {
        private IConfiguration config;
        public ServerController(IConfiguration _config)
        {
            this.config = _config;
        }
        [HttpPost("receive")]
        public IActionResult ReceiveNodeInfo([FromBody]ApiNodeManagerModel.NodeInfo info)
        {
            var node=NodePool.Get().AddOrUpdate(info);
            int interval= Convert.ToInt32(this.config.GetSection("MainConfig").GetValue<string>("HeartInterval"));
            var next_beat=DateTime.Now.AddSeconds(interval);
            object result = null;
            if (node.actionTask.Count > 0)
            {
                result = new { next = next_beat, action = node.actionTask.Dequeue() };
            }
            else
            {
                result = new { next = next_beat, action =""};
            }

            return ApiResult.OK(result);
        }
    }
}
