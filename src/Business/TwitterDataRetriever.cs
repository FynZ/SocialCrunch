using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using TweetSharp;

namespace Business
{
    public class TwitterDataRetriever
    {
        private const string consumerKey = "MyKfxtg9Qi5XkvHlvKq1phf5m";
        private const string consumerSecret = "aM4hsNAWLgn7jAMDKwYMJY2oCfKNVpXnkTYia1bel87bV34Jbp";

        private readonly TwitterService _twitter;
        private readonly IAuthenticatedUser _user;

        public TwitterDataRetriever(string token, string tokenSecret)
        {
            Auth.SetUserCredentials(consumerKey, consumerSecret, token, tokenSecret);
            _twitter = new TwitterService(token, tokenSecret);

            _user = User.GetAuthenticatedUser();
        }

        public object GetAnalytics()
        {
            var timeline = Tweetinvi.Timeline.GetUserTimeline(_user.Id, 2000);

            var likeCount = timeline.Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date)
                .Sum(x => x.TweetDTO.FavoriteCount.GetValueOrDefault());

            var retweetCount = timeline.Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date)
                .Sum(x => x.TweetDTO.RetweetCount);

            var followerCount = _user.FollowersCount;

            return new {likeCount, retweetCount, followerCount};
        }

        public int GetLikeCount()
        {
            var timeline = Tweetinvi.Timeline.GetUserTimeline(_user.Id, 2000);

            return timeline.Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date)
                .Sum(x => x.TweetDTO.FavoriteCount.GetValueOrDefault());
        }

        public object GetDailyAnalytics()
        {
            var timeline = Tweetinvi.Timeline.GetUserTimeline(_user.Id, 2000);

            var likeCount = timeline.Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date)
                .Sum(x => x.TweetDTO.FavoriteCount.GetValueOrDefault());

            var retweetCount = timeline.Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date)
                .Sum(x => x.TweetDTO.RetweetCount);

            var followerCount = _user.FollowersCount;

            var popularTweets = timeline.Where(x => x.IsRetweet).OrderByDescending(x => x, new TwitterComparer()).ToList().GetRange(0, 3);

            return new { likeCount, retweetCount, followerCount };
        }
    }
}