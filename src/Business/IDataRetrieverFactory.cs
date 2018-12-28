using Business.Facebook;
using Business.Twitter;
using Models.Facebook;
using Models.Twitter;

namespace Business
{
    public interface IDataRetrieverFactory
    {
        ITwitterDataRetriever GetTwitterDataRetriever(string token, string tokenSecret);
        IFacebookDataRetriever GetFacebookDataRetriever(string token);

        IDailyCollect<TwitterCompleteData> GetTwitterCollector(string token, string tokenSecret);
        IDailyCollect<FacebookCompleteData> GetFacebookCollector(string token);
    }
}