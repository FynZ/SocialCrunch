using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Twitter
{
    public class TwitterCompleteData
    {
        public TwitterDailySummary TwitterDailySummary { get; set; }
        public TwitterDailyData TwitterDailyData { get; set; }
        public IEnumerable<Tweet> BestDailyTweets { get; set; }
        public IEnumerable<Tweet> BestAllTimeTweets { get; set; }
    }
}
