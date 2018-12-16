using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Data.Token;
using Data.Twitter;
using Models.Enums;

namespace Business.Service
{
    public class TwitterService : IService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly ITwitterDataRepository _twitterDataRepository;
        private readonly TwitterDataRetriever _twitterDataRetriever;

        public SocialNetworkType Type { get; }
        public bool Running { get; private set; }

        private TimeSpan _workingDelay = new TimeSpan(0, 1, 0, 0);

        public TwitterService(
            ITokenRepository tokenRepository,
            ITwitterDataRepository twitterDataRepository,
            TwitterDataRetriever twitterRetriever,
            bool running = false)
        {
            _tokenRepository = tokenRepository;
            _twitterDataRepository = twitterDataRepository;
            _twitterDataRetriever = twitterRetriever;

            Type = SocialNetworkType.Twitter;
            Running = running;

            if (Running)
            {
                Start();
            }
        }

        public void Start()
        {
            if (!Running)
            {
                Run();
            }
        }

        public void Stop()
        {
            Running = false;
        }

        private async Task Run()
        {
            while (Running)
            {
                var watch = Stopwatch.StartNew();

                var tokens = await _tokenRepository.GetTokens(SocialNetworkType.Twitter);

                foreach (var token in tokens)
                {
                    _twitterDataRetriever.ChangeUser(token.AccessToken, token.TokenSecret);

                    var dailyData = _twitterDataRetriever.GetDailyAnalytics();
                    await _twitterDataRepository.InsertDailyData(dailyData, token.UserId);

                    var dailySummary = _twitterDataRetriever.GetDailySummary();
                    await _twitterDataRepository.InsertDailySummary(dailySummary, token.UserId);
                }

                // We wait the remaining time between cycles
                if (watch.Elapsed < _workingDelay)
                {
                    await Task.Delay(_workingDelay - watch.Elapsed);
                }
            }
        }
    }
}
