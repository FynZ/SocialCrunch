using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Twitter;
using Tweetinvi;
using Tweetinvi.Logic;
using Tweetinvi.Models;
using TweetSharp;

namespace Business
{
    /// <summary>
    /// Class Used to access the Twitter API through Tweetinvi
    /// <para>
    ///     /!\ THIS CLASS IS PROBABLY NOT THREAD SAFE LOOKING AT HOW THE AUTHENTICATION WORKS /!\
    ///     I know, fuck the developer who made this Api..
    /// </para>
    /// </summary>
    public class TwitterDataRetriever
    {
        private readonly TwitterService _twitter;
        private readonly IAuthenticatedUser _user;

        private readonly string _token;
        private readonly string _tokenSecret;

        public TwitterDataRetriever(string consumerKey, string consumerSecret, string token, string tokenSecret)
        {
            _token = token;
            _tokenSecret = tokenSecret;

            Auth.SetUserCredentials(consumerKey, consumerSecret, _token, _tokenSecret);

            _twitter = new TwitterService(token, tokenSecret);
            _user = Tweetinvi.User.GetAuthenticatedUser();
        }

        public bool IsSameUser(string token, string tokenSecret)
        {
            return String.CompareOrdinal(_token, token) == 0 && 
                   String.CompareOrdinal(_tokenSecret, tokenSecret) == 0;
        }

        public void CollectData()
        {
            var timeline = Timeline.GetUserTimeline(_user.Id, 2000);
        }

        public TwitterDailyData GetDailyAnalytics()
        {
            return GetDailyAnalyticsImpl();
        }

        private TwitterDailyData GetDailyAnalyticsImpl(IEnumerable<ITweet> timeline = null)
        {
            if (timeline == null) timeline = GetDailyTimeLine();

            int likeCount = 0;
            int retweetCount = 0;
            int repliesCount = 0;

            foreach (var tweet in timeline)
            {
                likeCount += tweet.TweetDTO.FavoriteCount.GetValueOrDefault();
                retweetCount += tweet.TweetDTO.RetweetCount;
                repliesCount += tweet.TweetDTO.ReplyCount.GetValueOrDefault();
            }

            return new TwitterDailyData(likeCount, retweetCount, repliesCount);
        }

        public TwitterDailySummary GetDailySummary()
        {
            var followers = _user.FollowersCount;
            var friends = _user.FriendsCount;
            var followings = _user.FriendsCount;
            var tweets = _user.StatusesCount;
            var likes = _user.FavouritesCount;

            return new TwitterDailySummary(followers, friends, followings, tweets, likes);
        }

        public IEnumerable<Models.Twitter.Tweet> GetBestTweets(IEnumerable<ITweet> timeline = null, int count = 3)
        {
            if (timeline == null) timeline = GetDailyTimeLine();

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

        private IEnumerable<ITweet> GetTimeLine(int count = 2000) => Timeline.GetUserTimeline(_user.Id, count);

        private IEnumerable<ITweet> GetDailyTimeLine() => Timeline.GetUserTimeline(_user.Id, 2000).Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date);
    }
}