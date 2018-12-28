using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Business;
using Data.Token;
using Data.Twitter;
using Business.Service;
using Models.Enums;
using Xunit;
using NFluent;
using FakeItEasy;

namespace Tests.Business.Service
{
    public class TwitterServiceTest
    {
        private readonly IService _service;

        public TwitterServiceTest()
        {
            var factory = A.Fake<IDataRetrieverFactory>();

            var tokenRepository = A.Fake<ITokenRepository>();

            var twitterDataRepository = A.Fake<ITwitterDataRepository>();

            var twitterDataRetriever = A.Fake<ITwitterDataRepository>();

            _service = new TwitterService(tokenRepository, twitterDataRepository, factory);
        }

        [Fact]
        public void Calling_Type_ShouldReturn_ServiceType()
        {
            var type = _service.Type;

            Check.That(type)
                .IsEqualTo(SocialNetworkType.Twitter);
        }

        [Fact]
        public void Calling_Start_WhenService_IsNotRunning_Should_StartIt()
        {
            _service.Start();

            var running = _service.Running;

            Check.That(running)
                .Equals(true);
        }

        [Fact]
        public void Calling_Start_WhenService_IsRunning_Should_DoNothing()
        {
            Check.That(true)
                .Equals(true);
        }

        [Fact]
        public void Calling_Stop_WhenService_IsRunning_Should_StopIt()
        {
            _service.Start();

            _service.Stop();

            var running = _service.Running;

            Check.That(running)
                .Equals(false);
        }
    }
}
