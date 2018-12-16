using System.Threading.Tasks;
using Models.Twitter;

namespace Data.Twitter
{
    public interface ITwitterDataRepository
    {
        Task<bool> InsertDailyData(TwitterDailyData data, int userId);
        Task<bool> InsertDailySummary(TwitterDailySummary data, int userId);
    }
}