using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Twitter;
using Tweetinvi;
using Tweetinvi.Logic;
using Tweetinvi.Models;

namespace Business.Twitter
{
    /// <summary>
    /// Class Used to access the Twitter API through Tweetinvi
    /// </summary>
    public class TwitterDataRetriever : ITwitterDataRetriever
    {
        private readonly string _token;
        private readonly string _tokenSecret;

        public IAuthenticatedUser User { get; }

        public TwitterDataRetriever(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            _token = token;
            _tokenSecret = tokenSecret;

            Auth.SetUserCredentials(consumerKey, consumerSecret, _token, _tokenSecret);

            User = Tweetinvi.User.GetAuthenticatedUser();
        }

        public bool IsSameUser(string token, string tokenSecret)
        {
            return String.CompareOrdinal(_token, token) == 0 && 
                   String.CompareOrdinal(_tokenSecret, tokenSecret) == 0;
        }

        public async Task<TwitterCompleteData> CollectData()
        {
            var timeline = await GetTimeLine();

            var holder = new TwitterCompleteData
            {
                TwitterDailySummary = GetDailySummary(),
                TwitterDailyData = await GetDailyAnalyticsImpl(timeline),
                BestDailyTweets = TwitterDataRetriever.GetBestTweets(TwitterDataRetriever.TimeLineToDailyTimeLine(timeline)),
                BestAllTimeTweets = TwitterDataRetriever.GetBestTweets(timeline),
            };

            return holder;
        }

        public async Task<TwitterDailyData> GetDailyAnalytics()
        {
            return await GetDailyAnalyticsImpl();
        }

        private async Task<TwitterDailyData> GetDailyAnalyticsImpl(IEnumerable<ITweet> timeline = null)
        {
            if (timeline == null) timeline = await GetDailyTimeLine();

            int likeCount = 0;
            int retweetCount = 0;
            int repliesCount = 0;

            foreach (var tweet in timeline)
            {
                likeCount += tweet.FavoriteCount;
                retweetCount += tweet.RetweetCount;
                repliesCount += tweet.ReplyCount.GetValueOrDefault();
            }

            return new TwitterDailyData(likeCount, retweetCount, repliesCount);
        }

        public TwitterDailySummary GetDailySummary()
        {
            var followers = User.FollowersCount;
            var friends = User.FriendsCount;
            var followings = 0;
            var tweets = User.StatusesCount;
            var likes = User.FavouritesCount;

            return new TwitterDailySummary(followers, friends, followings, tweets, likes);
        }

        public async Task<IEnumerable<Models.Twitter.Tweet>> GetBestDailyTweets(int count = 3)
        {
            return TwitterDataRetriever.GetBestTweets(await GetDailyTimeLine(), count);
        }

        public async Task<IEnumerable<Models.Twitter.Tweet>> GetAllTimeBestTweets(int count = 3)
        {
            return TwitterDataRetriever.GetBestTweets(await GetTimeLine(), count);
        }

        private static IEnumerable<Models.Twitter.Tweet> GetBestTweets(IEnumerable<ITweet> timeline, int count = 3)
        {
            return timeline
                .Where(x => x.IsRetweet)
                .OrderByDescending(x => x, new TwitterComparer())
                .ToList()
                .GetRange(0, count)
                .ConvertAll(x => new Models.Twitter.Tweet
                {
                    Id = x.Id,
                    UserId = x.CreatedBy.Id,
                    CreationDate = x.CreatedAt,
                    Content = x.FullText,
                    ReplyCount = x.ReplyCount.GetValueOrDefault(),
                    QuoteCount = x.QuoteCount.GetValueOrDefault(),
                    RetweetCount = x.RetweetCount,
                    LikeCount = x.FavoriteCount,
                    Url = x.Url
                });
        }

        private async Task<IEnumerable<ITweet>> GetTimeLine(int count = 2000) => await TimelineAsync.GetUserTimeline(User.Id, count);

        private async Task<IEnumerable<ITweet>> GetDailyTimeLine() => TwitterDataRetriever.TimeLineToDailyTimeLine(await GetTimeLine());

        private static IEnumerable<ITweet> TimeLineToDailyTimeLine(IEnumerable<ITweet> timeline) => timeline.Where(x => x.CreatedAt > DateTime.Now.Date);
    }
}