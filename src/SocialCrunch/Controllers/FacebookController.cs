using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Data;
using Data.Token;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;

namespace SocialCrunch.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1")]
    public class FacebookController : Controller
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IDataRetrieverFactory _dataRetrieverFactory;

        private readonly string _token;

        public FacebookController(ITokenRepository tokenRepository, IDataRetrieverFactory dataRetrieverFactory)
        {
            _tokenRepository = tokenRepository;
            _dataRetrieverFactory = dataRetrieverFactory;

            _token =
                "EAAKXvqExqpsBALSDorTKvpSglnZBX54X3CI1NTr4sBmXcT6Cb2GvlXylqkRT4f8AF0t5AZCkGJb0my0yxJz0WyAVn3m8XwSVco24shnEfzRvYDZAE8g9XFdY0rbxz4tqC2jGTZBVmA0TFTvFFWyUxNlPxIbjaO4ZD";
        }


        [HttpGet("user")]
        public async Task<IActionResult> UserProfile()
        {
            var tokens = await _tokenRepository.GetTokens(SocialNetworkType.Facebook);

            var retriever = _dataRetrieverFactory.GetFacebookDataRetriever(_token);
            var user = retriever.GetProfile();

            return Json(user);
        }

        [HttpGet("posts")]
        public async Task<IActionResult> Posts()
        {
            var tokens = await _tokenRepository.GetTokens(SocialNetworkType.Facebook);

            var retriever = _dataRetrieverFactory.GetFacebookDataRetriever(_token);
            var postDetails = new List<object>();

            return Json(retriever.GetFeed());
        }

        [HttpGet("comments")]
        public async Task<IActionResult> Comments()
        {
            var retriever = _dataRetrieverFactory.GetFacebookDataRetriever(_token);
            var comments = await retriever.GetUserComments();

            return Json(comments);
        }
    }
}
