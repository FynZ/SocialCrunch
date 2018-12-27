using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Twitter
{
    public class Tweet
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
        public int ReplyCount { get; set; }
        public int QuoteCount { get; set; }
        public int RetweetCount { get; set; }
        public int LikeCount { get; set; }
        public string Url { get; set; }
    }
}
