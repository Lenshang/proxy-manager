using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProxyManager.Core;
using ProxyManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyManager.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NodeManagerController : ControllerBase
    {
        private readonly ILogger logger;
        public NodeManagerController(ILogger<NodeManagerController> _logger)
        {
            this.logger = _logger;
        }

        [HttpGet("get-all-nodes")]
        public IActionResult GetAllNodes()
        {
            var result = new List<ApiNodeManagerModel.NodeInfo>();
            foreach(var node in NodePool.Get().Nodes)
            {
                result.Add(node.ToApiNodeInfo());
            }
            return ApiResult.OK(result);
        }

        [HttpGet("re-dial")]
        public IActionResult ReDial(string id)
        {
            return ApiResult.OK(NodePool.Get().ReDialNode(id));
        }
    }
}
