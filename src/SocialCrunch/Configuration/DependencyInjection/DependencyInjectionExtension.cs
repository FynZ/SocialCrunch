﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Business.Facebook;
using Business.Service;
using Business.Twitter;
using Data;
using Data.Token;
using Data.Twitter;
using Microsoft.Extensions.DependencyInjection;

namespace SocialCrunch.Configuration.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection InjectRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<ITokenRepository, TokenRepository>(x => new TokenRepository(connectionString));
            services.AddSingleton<ITwitterDataRepository, TwitterDataRepository>(x => new TwitterDataRepository(connectionString));

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            services.AddSingleton<ITwitterDataRetriever, TwitterDataRetriever>();
            services.AddSingleton<IFacebookDataRetriever, FacebookDataRetriever>();

            services.AddSingleton<IDataRetrieverFactory, DataRetrieverFactory>();

            services.AddSingleton<TwitterService, TwitterService>();
            services.AddSingleton<FacebookService, FacebookService>();

            services.AddSingleton<IServiceManager, ServiceManager>();

            return services;
        }

        public static IServiceCollection InjectUtils(this IServiceCollection services)
        {
            return services;
        }
    }
}
