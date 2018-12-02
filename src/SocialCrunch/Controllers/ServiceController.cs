using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SocialCrunch.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2")]
    public class ServiceController : Controller
    {
        /// <summary>
        /// I can even describe what the endpoint does
        /// </summary>
        /// <returns></returns>
        [HttpGet("start")]
        public IActionResult StartService()
        {
            return Json(new {PlaceHolder = "Not Yet Implemented"});
        }

        /// <summary>
        /// Such amazing
        /// </summary>
        /// <returns></returns>
        [HttpGet("stop")]
        public IActionResult StopService()
        {
            return Json(new { PlaceHolder = "Not Yet Implemented" });
        }

        /// <summary>
        /// Much wow
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/run/{id}")]
        public IActionResult RunForUser(int userId)
        {
            return Json(new { PlaceHolder = "Not Yet Implemented" });
        }
    }
}