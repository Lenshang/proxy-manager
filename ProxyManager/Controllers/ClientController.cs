using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
    [ApiController]
    [AllowAnonymous]
    public class ClientController : ControllerBase
    {
        private IConfiguration config;
        private readonly ILogger logger;
        public ClientController(IConfiguration _config, ILogger<ClientController> _logger)
        {
            this.config = _config;
            this.logger = _logger;
        }
        [HttpGet("get")]
        public IActionResult GetOne()
        {
            var node = NodePool.Get().GetOne();
            if (node != null)
            {
                return ApiResult.OK(node.ToApiNodeInfo());
            }
            return ApiResult.Failure();
        }
        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var result = NodePool.Get().GetAll().Select(i=>i.ToApiNodeInfo()).ToArray();
            if (result.Length > 0)
            {
                return ApiResult.OK(result);
            }
            else
            {
                return ApiResult.Failure();
            }
        }
        [HttpGet("redial")]
        public IActionResult Redial(string id)
        {
            if (NodePool.Get().ReDialNode(id))
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }
        [HttpGet("redial-all")]
        public IActionResult RedialAll(string id)
        {
            if (NodePool.Get().ReDialAll())
            {
                return ApiResult.OK();
            }
            else
            {
                return ApiResult.Failure();
            }
        }
    }
}
