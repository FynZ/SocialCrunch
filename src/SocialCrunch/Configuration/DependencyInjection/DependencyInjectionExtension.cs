using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.Extensions.DependencyInjection;

namespace SocialCrunch.Configuration.DependencyInjection
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection InjectRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<ITokenRepository, TokenRepository>(x => new TokenRepository(connectionString));

            return services;
        }

        public static IServiceCollection InjectServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection InjectUtils(this IServiceCollection services)
        {
            return services;
        }
    }
}
