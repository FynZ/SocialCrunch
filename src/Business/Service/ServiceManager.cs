using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Token;
using Data.Twitter;
using Models.Enums;

namespace Business.Service
{
    public class ServiceManager
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

        private readonly TimeSpan _workingDelay;
        private readonly Dictionary<SocialNetworkType, IService> _services;

        public bool TwitterRunning { get; set; }

        public ServiceManager(TwitterService twitterService, FacebookService facebookService)
        {
            _services.Add(SocialNetworkType.Twitter, twitterService);
            _services.Add(SocialNetworkType.Facebook, facebookService);
            _services.Add(SocialNetworkType.Instagram, null);
            _services.Add(SocialNetworkType.GooglePlus, null);
            _services.Add(SocialNetworkType.Linkedin, null);
            _services.Add(SocialNetworkType.MySpace, null);
            _services.Add(SocialNetworkType.Github, null);
            _services.Add(SocialNetworkType.MySpace, null);
        }
    }
}
