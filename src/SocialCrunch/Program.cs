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

        private static string Environement =>
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "development";

        private static IConfiguration Configuration =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environement}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

        public static int Main(string[] args)
        {
            try
            {
                Log.Information("Starting web host for {environment}", Environement);

                WebHost.CreateDefaultBuilder(args)
                    .UseKestrel()
                    //.UseLibuv()
                    .UseStartup<Startup>()
                    .UseConfiguration(Configuration)
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
