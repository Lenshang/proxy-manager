using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProxyManager.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyManager.Controllers
{
    public class SaveConfigModel
    {
        public int heart_interval { get; set; }
        public bool auto_redial { get; set; }
        public int redial_interval { get; set; }
    }
    [Route("api")]
    [Authorize]
    [ApiController]
    public class MainController : ControllerBase
    {
        private IConfiguration config;
        public MainController(IConfiguration _config)
        {
            this.config = _config;
        }
        [HttpGet("check-login")]
        public IActionResult CheckLogin()
        {
            return ApiResult.OK();
        }
        [HttpGet("api-info")]
        public IActionResult ApiInfo()
        {
            List<object> result = new List<object>();
            result.Add(new { name = "api/client/get", content = "随机获得一条HTTP代理节点" });
            result.Add(new { name = "api/client/get-all", content = "随机全部HTTP代理节点" });
            result.Add(new { name = "api/client/redial?id=", content = "强制拨号一个代理节点" });
            result.Add(new { name = "api/client/redial-all", content = "强制拨号所有节点" });
            return ApiResult.OK(result);
        }
        [HttpGet("get-config")]
        public IActionResult GetConfig()
        {
            var c=this.config.GetSection("MainConfig");

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("heart_interval", c.GetValue<string>("HeartInterval"));
            result.Add("auto_redial", c.GetValue<string>("AutoRedial"));
            result.Add("redial_interval", c.GetValue<string>("RedialInterval"));
            return ApiResult.OK(result);
        }
        [HttpPost("save-config")]
        public IActionResult SaveConfig([FromBody]SaveConfigModel _config)
        {
            this.config.GetSection("MainConfig").GetSection("HeartInterval").Value= _config.heart_interval.ToString();
            this.config.GetSection("MainConfig").GetSection("AutoRedial").Value = _config.auto_redial.ToString();
            this.config.GetSection("MainConfig").GetSection("RedialInterval").Value = _config.redial_interval.ToString();
            return ApiResult.OK();
        }
    }
}
