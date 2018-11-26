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

namespace SocialCrunch
{
    public class Program
    {
        public static IConfiguration Configuration { get; }

        public static int Main(string[] args)
        {
            try
            {
                Log.Information("Starting web host for {environment}", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development");

                WebHost.CreateDefaultBuilder(args)
                    //.UseLibuv()
                    .UseStartup<Startup>()
                    .UseConfiguration(Configuration)
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
