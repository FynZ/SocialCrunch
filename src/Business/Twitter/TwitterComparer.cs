using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Models;

namespace Business.Twitter
{
    class TwitterComparer : IComparer<ITweet>
    {
        private readonly int _favoriteWeight;
        private readonly int _retweetWeight;
        private readonly int _commentWeight;

        public TwitterComparer() : this(1, 2, 4) { }

        public TwitterComparer(int favoriteWeight, int retweetWeight, int commentWeight)
        {
            _favoriteWeight = favoriteWeight;
            _retweetWeight = retweetWeight;
            _commentWeight = commentWeight;
        }

        public int Compare(ITweet x, ITweet y)
        {
            var xPow = x.TweetDTO.FavoriteCount * _favoriteWeight
                       + x.TweetDTO.RetweetCount * _retweetWeight
                       + x.TweetDTO.ReplyCount.GetValueOrDefault() * _commentWeight;

            var yPow = y.TweetDTO.FavoriteCount * _favoriteWeight
                       + y.TweetDTO.RetweetCount * _retweetWeight
                       + y.TweetDTO.ReplyCount.GetValueOrDefault() * _commentWeight;


            if (xPow > yPow)
            {
                return 1;
            }
            else if (xPow < yPow)
            {
                return -1;
            }

            return 0;
        }
    }
}
