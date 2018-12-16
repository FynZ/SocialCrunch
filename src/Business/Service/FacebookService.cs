using System;
using System.Collections.Generic;
using System.Text;
using Models.Enums;

namespace Business.Service
{
    public class FacebookService : IService
    {
        public SocialNetworkType Type { get; }
        public bool Running { get; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
