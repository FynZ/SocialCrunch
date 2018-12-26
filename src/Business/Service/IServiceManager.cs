using System;
using System.Collections.Generic;
using System.Text;
using Models.Enums;

namespace Business.Service
{
    public interface IServiceManager
    {
        void StartServices();
        void StopServices();

        bool StartService(SocialNetworkType target);
        bool StopService(SocialNetworkType target);

        bool IsServiceRunning(SocialNetworkType target);
    }
}
