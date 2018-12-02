using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Data;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialCrunch.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class FacebookController : Controller
    {
        private readonly ITokenRepository _tokenRepository;

        private readonly string _token;

        public FacebookController(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;

            _token =
                "EAAKXvqExqpsBALSDorTKvpSglnZBX54X3CI1NTr4sBmXcT6Cb2GvlXylqkRT4f8AF0t5AZCkGJb0my0yxJz0WyAVn3m8XwSVco24shnEfzRvYDZAE8g9XFdY0rbxz4tqC2jGTZBVmA0TFTvFFWyUxNlPxIbjaO4ZD";
        }


        [HttpGet("user")]
        public async Task<IActionResult> UserProfile()
        {
            var tokens = await _tokenRepository.GetTokens(SocialNetworkType.Facebook);

            var retriever = new FacebookDataRetriever(_token);
            var user = retriever.GetProfile();

            return Json(user);
        }

        [HttpGet("posts")]
        public async Task<IActionResult> Posts()
        {
            var tokens = await _tokenRepository.GetTokens(SocialNetworkType.Facebook);

            var retriever = new FacebookDataRetriever(_token);

            var postDetails = new List<object>();

            return Json(retriever.GetFeed());
        }

        [HttpGet("comments")]
        public async Task<IActionResult> Comments()
        {
            var retriever = new FacebookDataRetriever(_token);

            var comments = retriever.GetUserComments();

            return Json(comments);
        }
    }
}
