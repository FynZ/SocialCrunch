using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Business;
using Business.Twitter;

namespace SocialCrunch.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class TwitterController : Controller
    {
        // to remove after
        private readonly string _token = "3405556169-aLsMP8geNwVP80gK3CmF4wsWjTMAiPE0LEnfkfK";
        private readonly string _tokenSecret = "dXULm8t2XpHa0t9rHeZFVx58m1ZQT721z84N7O1vvypKx";

        private readonly IDataRetrieverFactory _dataRetrieverFactory;

        public TwitterController(IDataRetrieverFactory dataRetrieverFactory)
        {
            _dataRetrieverFactory = dataRetrieverFactory;
        }

        [HttpGet("daily-analytics")]
        public IActionResult GetDailyAnalytics()
        {
            var retriever = _dataRetrieverFactory.GetTwitterDataRetriever(_token, _tokenSecret);

            var a = retriever.GetDailyAnalytics();

            return Json(a);
        }

        [HttpGet("daily-summary")]
        public IActionResult GetDailySummary()
        {
            var retriever = _dataRetrieverFactory.GetTwitterDataRetriever(_token, _tokenSecret);

            var a = retriever.GetDailySummary();

            return Json(a);
        }
    }
}