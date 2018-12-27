using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Twitter;

namespace Data.Twitter
{
    public interface ITwitterDataRepository
    {
        /// <summary>
        /// Returns a <see cref="TwitterDailyData"/> for the given user and the given day if provided, otheriwse of today
        /// </summary>
        /// <param name="userId">Application user Id</param>
        /// <param name="day">The day of the record, today if not provided</param>
        /// <returns>The correspondig <see cref="TwitterDailyData"/> object</returns>
        Task<TwitterDailyData> GetDailyData(int userId, DateTime? day = null);

        /// <summary>
        /// Insert the given <see cref="TwitterDailyData"/> for the given user
        /// </summary>
        /// <param name="userId">Application user Id</param>
        /// <param name="data">The data to insert</param>
        /// <returns>True on success, otherwise false</returns>
        Task<bool> InsertDailyData(int userId, TwitterDailyData data);

        /// <summary>
        /// Insert the given <see cref="TwitterDailySummary"/> for the given user
        /// </summary>
        /// <param name="userId">Application user Id</param>
        /// <param name="data">The data to insert</param>
        /// <returns>True on success, otherwise false</returns>
        Task<bool> InsertDailySummary(int userId, TwitterDailySummary data);

        /// <summary>
        /// Insert the best given <see cref="Tweet"/> as most performing daily Tweets for the given user
        /// </summary>
        /// <param name="userId">Application user Id</param>
        /// <param name="tweets">The Tweets to insert</param>
        /// <returns>True on success, otherwise false</returns>
        Task<bool> InsertBestDailyTweets(int userId, IEnumerable<Tweet> tweets);

        /// <summary>
        /// Insert the best given <see cref="Tweet"/> as all time most performing Tweets for the given user
        /// </summary>
        /// <param name="userId">Application user Id</param>
        /// <param name="tweets">The Tweets to insert</param>
        /// <returns>True on success, otherwise false</returns>
        Task<bool> InsertBestTweets(int userId, IEnumerable<Tweet> tweets);
    }
}