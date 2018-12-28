using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Business.Twitter;
using Data.Token;
using Data.Twitter;
using Models;
using Models.Enums;
using Serilog;

namespace Business.Service
{
    public class TwitterService : IService
    {
        public SocialNetworkType Type { get; }
        public bool Running { get; private set; }

        private readonly ITokenRepository _tokenRepository;
        private readonly ITwitterDataRepository _twitterDataRepository;
        private readonly IDataRetrieverFactory _factory;

        private readonly TimeSpan _workingDelay;
        private bool _forceStop = false;

        public TwitterService(
            ITokenRepository tokenRepository,
            ITwitterDataRepository twitterDataRepository,
            IDataRetrieverFactory dataRetrieverFactory,
            bool running = false)
        {
            _tokenRepository = tokenRepository;
            _twitterDataRepository = twitterDataRepository;
            _factory = dataRetrieverFactory;

            Type = SocialNetworkType.Twitter;
            Running = running;

            _workingDelay = new TimeSpan(0, 1, 0, 0);

            if (Running)
            {
                Start();
            }
        }

        public void Start()
        {
            if (!Running)
            {
                Log.Information($"Starting service {Type}");

                Running = true;
                _forceStop = false;

                _ = Run(); // use wildcard to prevent warning (no preprocess directive, no suppress warning
            }
        }

        public void Stop()
        {
            Log.Information($"Stopping service {Type}");

            Running = false;
        }

        public void ForceStop()
        {
            Log.Information($"Force stopping service {Type}");

            _forceStop = true;
        }

        private async Task Run()
        {
            try
            {
                while (Running)
                {
                    var watch = Stopwatch.StartNew();

                    var tokens = await _tokenRepository.GetTokens(SocialNetworkType.Twitter);

                    foreach (var token in tokens)
                    {
                        if (!_forceStop)
                        {
                            await RunForToken(token);
                        }
                        else
                        {
                            // Setting value back to false
                            _forceStop = false;
                            break;
                        }
                    }

                    // We wait the remaining time between cycles
                    if (watch.Elapsed < _workingDelay && Running)
                    {
                        await Task.Delay(_workingDelay - watch.Elapsed);
                    }
                    else
                    {
                        Running = false;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Fatal($"Unhandled error in loop for service {Type} with exception {e.Message}", e);
            }
        }

        private async Task RunForToken(SocialToken token)
        {
            try
            {
                var twitterDataRetriever = _factory.GetTwitterDataRetriever(token.AccessToken, token.TokenSecret);

                var gatheredData = await twitterDataRetriever.CollectData();

                var previousRun = await _twitterDataRepository.GetDailyData(token.UserId, DateTime.Now.AddDays(-1));

                // if we have data from the previous day, we get the difference, else it's the value of today
                gatheredData.TwitterDailyData.Followers = previousRun != null
                    ? twitterDataRetriever.User.FollowersCount - previousRun.Followers
                    : twitterDataRetriever.User.FollowersCount;

                await _twitterDataRepository.InsertDailyData(token.UserId, gatheredData.TwitterDailyData);
                Log.Information($"Daily data inserted for user {token.UserId}");

                await _twitterDataRepository.InsertDailySummary(token.UserId, gatheredData.TwitterDailySummary);
                Log.Information($"Daily summary inserted for user {token.UserId}");

                await _twitterDataRepository.InsertBestDailyTweets(token.UserId, gatheredData.BestDailyTweets);
                Log.Information($"Best daily tweets inserted for user {token.UserId}");

                await _twitterDataRepository.InsertBestTweets(token.UserId, gatheredData.BestAllTimeTweets);
                Log.Information($"Best all time tweets inserted for user {token.UserId}");
            }
            catch (Exception e)
            {
                Log.Error($"An error occured when gathering data for user {token.UserId} with exception {e.Message}", e);
            }
        }
    }
}
