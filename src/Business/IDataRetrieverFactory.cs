using Business.Facebook;
using Business.Twitter;

namespace Business
{
    public interface IDataRetrieverFactory
    {
        ITwitterDataRetriever GetTwitterDataRetriever(string token, string tokenSecret);
        FacebookDataRetriever GetFacebookDataRetriever(string token);
    }
}