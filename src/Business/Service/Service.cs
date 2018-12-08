using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data;
using Models.Enums;

namespace Business.Service
{
    public class Service
    {
        #region SingletonImplementation
        //private static volatile Service INSTANCE;
        //private static readonly object Locker = new object();

        //protected Service() { }

        //public static Service GetService()
        //{
        //    if (INSTANCE != null)
        //    {
        //        return INSTANCE;
        //    }

        //    lock (Locker)
        //    {
        //        if (INSTANCE != null)
        //        {
        //            return INSTANCE;
        //        }

        //        var instance = new Service();

        //        INSTANCE = instance;

        //        return INSTANCE;
        //    }
        //}
        #endregion SingletonImplementation

        private readonly ITokenRepository _tokenRepository;
        private readonly TwitterDataRetriever _twitterDataRetriever;
        private readonly FacebookDataRetriever _facebookDataRetriever;

        public bool Working { get; set; }

        public Service(ITokenRepository tokenRepository, TwitterDataRetriever twitterRetriever, FacebookDataRetriever facebookDataRetriever)
        {
            _tokenRepository = tokenRepository;

            _twitterDataRetriever = twitterRetriever;
            _facebookDataRetriever = facebookDataRetriever;
        }

        public void DoStuff()
        {
            var tokens = _tokenRepository.GetTokens();
        }

        public async Task DoStuff(SocialNetworkType target)
        {
            while (Working)
            {
                var tokens = await _tokenRepository.GetTokens(target);

                foreach (var token in tokens)
                {
                    _twitterDataRetriever.ChangeUser(token.AccessToken, token.TokenSecret);
                    var data = _twitterDataRetriever.GetAnalytics();


                }
            }
        }
    }
}
