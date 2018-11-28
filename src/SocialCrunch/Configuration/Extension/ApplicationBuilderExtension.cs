using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using SocialCrunch.Configuration.Middleware;

namespace SocialCrunch.Configuration.Extension
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder AddLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<SerilogMiddleware>();

            return app;
        }

        public static IApplicationBuilder InitializeApiDocumentation(this IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (!env.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });
            }

            return app;
        }
    }
}
