using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

namespace SocialCrunch.Configuration.Extension
{
    public static class WebHostBuilderExtension
    {
        public static IWebHostBuilder AddLogging(this IWebHostBuilder builder)
        {
            builder.UseSerilog();

            return builder;
        }

        public static IWebHostBuilder AddMetrics(this IWebHostBuilder builder)
        {
            builder.UseHealthEndpoints()
                .UseMetricsWebTracking()
                .UseMetrics(options =>
                {
                    options.EndpointOptions = endpointsOptions =>
                    {
                        endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                        endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                    };
                });

            return builder;
        }
    }
}
