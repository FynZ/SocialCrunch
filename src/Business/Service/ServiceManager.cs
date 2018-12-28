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
    public class ServiceManager : IServiceManager
    {
        private readonly Dictionary<SocialNetworkType, IService> _services;

        public ServiceManager(TwitterService twitterService, FacebookService facebookService)
        {
            _services = new Dictionary<SocialNetworkType, IService>
            {
                {SocialNetworkType.Twitter, twitterService},
                {SocialNetworkType.Facebook, facebookService},
                {SocialNetworkType.Instagram, null},
                {SocialNetworkType.GooglePlus, null},
                {SocialNetworkType.Linkedin, null},
                {SocialNetworkType.MySpace, null},
                {SocialNetworkType.Github, null},
                {SocialNetworkType.Youtube, null}
            };
        }

        public void StartServices()
        {
            foreach (var service in _services.Values)
            {
                if (!service.Running)
                {
                    service?.Start();
                }
            }
        }

        public void StopServices()
        {
            foreach (var service in _services.Values)
            {
                if (service.Running)
                {
                    service.Stop();
                }
            }
        }


        public bool StartService(SocialNetworkType target)
        {
            if (_services.TryGetValue(target, out IService service))
            {
                if (!service.Running)
                {
                    service.Start();
                }

                return true;
            }

            return false;
        }

        public bool StopService(SocialNetworkType target)
        {
            if (_services.TryGetValue(target, out IService service))
            {
                if (service.Running)
                {
                    service.Stop();
                }

                return true;
            }

            return false;
        }

        public bool IsServiceRunning(SocialNetworkType target)
        {
            if (_services.TryGetValue(target, out IService service))
            {
                if (service.Running)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
