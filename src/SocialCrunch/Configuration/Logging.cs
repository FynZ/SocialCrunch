using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace SocialCrunch.Configuration
{
    public class Logging
    {
        public static void InitializeLogging(IConfiguration configuration)
        {
            SelfLog.Enable(Console.Error);

            var assembly = Assembly.GetEntryAssembly();

            var logCfg = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Application", assembly.GetName().Name)
                .Enrich.WithProperty("Version", FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion)
                .Enrich.WithProperty("Environment", configuration["ASPNETCORE_ENVIRONMENT"])
                .WriteTo.Console();

            var elasticSearchUri = configuration["ElasticSearchUri"];
            var indexPrefixTemplate = configuration["ElasticSearchIndexPrefix"];

            if (!string.IsNullOrEmpty(elasticSearchUri) && !string.IsNullOrEmpty(indexPrefixTemplate))
            {
                Console.WriteLine("Elastic Logs will be stored at " + elasticSearchUri);
                logCfg.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUri))
                {
                    AutoRegisterTemplate = true,
                    BatchPostingLimit = 50,
                    InlineFields = true,
                    MinimumLogEventLevel = LogEventLevel.Debug,
                    BufferFileSizeLimitBytes = 5242880,
                    IndexFormat = indexPrefixTemplate + "-{0:yyyy.MM}"
                });
            }

            Serilog.Log.Logger = logCfg.CreateLogger();
        }
    }
}
