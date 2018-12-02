using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Microsoft.AspNetCore.Mvc;

namespace SocialCrunch.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class TwitterController : Controller
    {
        private readonly string _token;
        private readonly string _tokenSecret;

        public TwitterController()
        {
            _token = "3405556169-aLsMP8geNwVP80gK3CmF4wsWjTMAiPE0LEnfkfK";
            _tokenSecret = "dXULm8t2XpHa0t9rHeZFVx58m1ZQT721z84N7O1vvypKx";
        }

        [HttpGet("tweets")]
        public IActionResult Index()
        {
            var retriever = new TwitterDataRetriever(_token, _tokenSecret);

            var a = retriever.GetAnalytics();

            return Json(a);
        }

        [HttpGet("like-count")]
        public IActionResult LikeCount()
        {
            var retriever = new TwitterDataRetriever(_token, _tokenSecret);

            var a = retriever.GetLikeCount();

            return Json(a);
        }
    }
}