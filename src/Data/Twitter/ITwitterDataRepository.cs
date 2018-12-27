using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Twitter;

namespace Data.Twitter
{
    public interface ITwitterDataRepository
    {
        Task<bool> InsertDailyData(TwitterDailyData data, int userId);
        Task<bool> InsertDailySummary(TwitterDailySummary data, int userId);
        Task<bool> InsertBestDailyTweets(IEnumerable<Tweet> tweets, int userId);
        Task<bool> InsertBestTweets(IEnumerable<Tweet> tweets, int userId);
        Task<TwitterDailyData> GetDailyData(int userId, DateTime? day = null);
    }
}