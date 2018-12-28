using System;
using System.Collections.Generic;
using System.Text;
using Models.Enums;

namespace Business.Service
{
    public interface IService
    {
        SocialNetworkType Type { get; }
        bool Running { get; }

        void Start();
        void Stop();
        void ForceStop();
    }
}
