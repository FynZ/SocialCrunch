using System;
using System.Collections.Generic;
using System.Text;
using Business.Facebook;
using Business.Twitter;
using Microsoft.Extensions.Configuration;

namespace Business
{
    public class DataRetrieverFactory : IDataRetrieverFactory
    {
        private readonly string _twitterConsumerKey;
        private readonly string _twitterConsumerSecret;

        private readonly object _twitterLock = new object();
        private readonly object _facebookLock = new object();

        private TwitterDataRetriever _twitterDataRetrieverCachedInstance;
        private FacebookDataRetriever _facebookDataRetrieverCachedInsstance;

        public DataRetrieverFactory(IConfiguration configuration)
        {
            _twitterConsumerKey = configuration["TwitterConsumerKey"] ?? "MyKfxtg9Qi5XkvHlvKq1phf5m";
            _twitterConsumerSecret = configuration["TwitterConsumerSecret"] ?? "aM4hsNAWLgn7jAMDKwYMJY2oCfKNVpXnkTYia1bel87bV34Jbp";
        }

        public TwitterDataRetriever GetTwitterDataRetriever(string token, string tokenSecret)
        {
            if (_twitterDataRetrieverCachedInstance == null || 
                !_twitterDataRetrieverCachedInstance.IsSameUser(token, tokenSecret))
            {
                lock (_twitterLock)
                {
                    // create and cache a new object if it doesn't exist or if the user is not the same
                    _twitterDataRetrieverCachedInstance = new TwitterDataRetriever(token, tokenSecret, _twitterConsumerKey, _twitterConsumerSecret);
                }
            }

            return _twitterDataRetrieverCachedInstance;
        }

        public FacebookDataRetriever GetFacebookDataRetriever(string token)
        {
            if (_facebookDataRetrieverCachedInsstance == null ||
                !_facebookDataRetrieverCachedInsstance.IsSameUser(token))
            {
                lock (_facebookLock)
                {
                    // create and cache a new object if it doesn't exist or if the user is not the same
                    _facebookDataRetrieverCachedInsstance = new FacebookDataRetriever(token);
                }
            }

            return _facebookDataRetrieverCachedInsstance;
        }
    }
}
