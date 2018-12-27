using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Twitter
{
    public class TwitterDailyData
    {
        public int Likes { get; set; }
        public int Retweets { get; set; }
        public int Replies { get; set; }
        public int Followers { get; set; }

        public TwitterDailyData(int likes, int retweets, int replies)
        {
            Likes = likes;
            Retweets = retweets;
            Replies = replies;
        }
    }
}
