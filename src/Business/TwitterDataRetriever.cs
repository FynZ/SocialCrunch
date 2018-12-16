using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models.Twitter;
using Tweetinvi;
using Tweetinvi.Models;
using TweetSharp;

namespace Business
{
    public class TwitterDataRetriever
    {
        private const string consumerKey = "MyKfxtg9Qi5XkvHlvKq1phf5m";
        private const string consumerSecret = "aM4hsNAWLgn7jAMDKwYMJY2oCfKNVpXnkTYia1bel87bV34Jbp";

        private TwitterService _twitter;
        private IAuthenticatedUser _user;

        public TwitterDataRetriever()
        {

        }

        //public TwitterDataRetriever(string token, string tokenSecret)
        //{
        //    Auth.SetUserCredentials(consumerKey, consumerSecret, token, tokenSecret);

        //    _twitter = new TwitterService(token, tokenSecret);
        //    _user = User.GetAuthenticatedUser();
        //}

        public TwitterDataRetriever ChangeUser(string token, string tokenSecret)
        {
            Auth.SetUserCredentials(consumerKey, consumerSecret, token, tokenSecret);

            _twitter = new TwitterService(token, tokenSecret);
            _user = User.GetAuthenticatedUser();

            return this;
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

        public TwitterDailyData GetDailyAnalytics()
        {
            var timeline = Tweetinvi.Timeline.GetUserTimeline(_user.Id, 2000)
                .Where(x => x.TweetDTO.CreatedAt > DateTime.Now.Date);

            int likeCount = 0;
            int retweetCount = 0;
            int repliesCount = 0;

            foreach (var tweet in timeline)
            {
                likeCount += tweet.TweetDTO.FavoriteCount.GetValueOrDefault();
                retweetCount += tweet.TweetDTO.RetweetCount;
                repliesCount += tweet.TweetDTO.ReplyCount.GetValueOrDefault();
            }

            var followerCount = _user.FollowersCount;

            var popularTweets = timeline.Where(x => x.IsRetweet).OrderByDescending(x => x, new TwitterComparer()).ToList().GetRange(0, 3);

            return new TwitterDailyData(likeCount, retweetCount, repliesCount);
        }

        public TwitterDailySummary GetDailySummary()
        {
            var user = Tweetinvi.User.GetAuthenticatedUser();

            var followers = user.FollowersCount;
            var friends = user.FriendsCount;
            var followings = user.FriendsCount;
            var tweets = user.StatusesCount;
            var likes = user.FavouritesCount;

            return new TwitterDailySummary(followers, friends, followings, tweets, likes);
        }
    }
}