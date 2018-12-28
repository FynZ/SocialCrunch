using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Twitter;
using Tweetinvi.Models;

namespace Business.Twitter
{
    public interface ITwitterDataRetriever : IDailyCollect<TwitterCompleteData>
    {
        IAuthenticatedUser User { get; }

        bool IsSameUser(string token, string tokenSecret);

        Task<TwitterDailyData> GetDailyAnalytics();
        TwitterDailySummary GetDailySummary();

        Task<IEnumerable<Models.Twitter.Tweet>> GetBestDailyTweets(int count = 3);
        Task<IEnumerable<Models.Twitter.Tweet>> GetAllTimeBestTweets(int count = 3);
    }
}
