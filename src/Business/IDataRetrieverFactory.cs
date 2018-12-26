namespace Business
{
    public interface IDataRetrieverFactory
    {
        TwitterDataRetriever GetTwitterDataRetriever(string token, string tokenSecret);
        FacebookDataRetriever GetFacebookDataRetriever(string token);
    }
}