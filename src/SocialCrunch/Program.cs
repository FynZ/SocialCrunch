using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using SocialCrunch.Configuration;
using SocialCrunch.Configuration.Extension;

namespace SocialCrunch
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development";

            try
            {
                Log.Information("Starting web host for {environment}", environment);

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                Logging.InitializeLogging(configuration);

                WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    //.UseLibuv()
                    .UseStartup<Startup>()
                    .UseConfiguration(configuration)
                    .UseUrls("http://*:80")
                    .AddMetrics()
                    .AddLogging()
                    .Build()
                    .Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
