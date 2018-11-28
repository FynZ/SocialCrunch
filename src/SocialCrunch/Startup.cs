using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Converters;
using SocialCrunch.Configuration;
using SocialCrunch.Configuration.Extension;

namespace SocialCrunch
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddApiVersioning(options => options.ReportApiVersions = true);
            services.AddMvcCore().AddVersionedApiExplorer(o => o.GroupNameFormat = "'v'V");

            services.ConfigureApiDocumentation("SocialCrunch API",
                Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "SocialCrunch.xml"));

            // Mvc behavior
            services.AddMvc(options =>
                {
                    // Filters
                    options.Filters.Add(new ResponseCacheAttribute()
                    {
                        Location = ResponseCacheLocation.None,
                        NoStore = true
                    });
                    options.Filters.Add(new ProducesAttribute("application/json"));
                })
                .AddJsonOptions(options =>
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true)))
                .AddMvcOptions(o => o.ModelMetadataDetailsProviders.Add(new DataAnnotationsMetadataProvider()));

            // Api behavior
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory =
                    ctx => throw new InvalidModelStateException(ctx.ModelState);
            });

            // GZip compression
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var forwardedOptions = new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All };
            forwardedOptions.KnownNetworks.Clear();
            forwardedOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardedOptions)
                .UseResponseCompression()
                .AddLoggingMiddleware()
                .UseMvc()
                .InitializeApiDocumentation(env, provider);

            //app.UseHttpsRedirection();
            //app.UseMvc();
        }
    }
}
