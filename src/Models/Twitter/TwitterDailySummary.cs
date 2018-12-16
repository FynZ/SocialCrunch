using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Twitter
{
    public class TwitterDailySummary
    {
        public int Followers { get; set; }
        public int Followings { get; set; }
        public int Friends { get; set; }
        public int Tweets { get; set; }
        public int Likes { get; set; }

        public TwitterDailySummary(int followers, int followings, int friends, int tweets, int likes)
        {
            Followers = followers;
            Followings = followings;
            Friends = friends;
            Tweets = tweets;
            Likes = likes;
        }
    }
}
