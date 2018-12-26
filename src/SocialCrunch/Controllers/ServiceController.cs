using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Service;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;

namespace SocialCrunch.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class ServiceController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public ServiceController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("start")]
        public IActionResult StartAll()
        {
            _serviceManager.StartServices();

            return Ok();
        }

        [HttpGet("stop")]
        public IActionResult StopAll()
        {
            _serviceManager.StopServices();

            return Ok();
        }

        [HttpGet("start/{service}")]
        public IActionResult StartService(SocialNetworkType service)
        {
            return Json(_serviceManager.StartService(service));
        }

        [HttpGet("stop/{service}")]
        public IActionResult StopService(SocialNetworkType service)
        {
            return Json(_serviceManager.StopService(service));
        }

        [HttpGet("running/{service}")]
        public IActionResult IsServiceRunning(SocialNetworkType service)
        {
            return Json(_serviceManager.IsServiceRunning(service));
        }
    }
}