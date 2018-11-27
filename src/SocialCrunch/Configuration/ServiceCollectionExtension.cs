using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Swashbuckle.AspNetCore.Swagger;

namespace SocialCrunch.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static void ConfigureApiDocumentation(this IServiceCollection services, string title, string path)
        {
            services.AddSwaggerGen(options =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var desc in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(desc.GroupName, new Info
                    {
                        Title = title + " V" + desc.ApiVersion,
                        Version = desc.ApiVersion.ToString(),
                        Description = desc.IsDeprecated ? "This API version has been deprecated." : ""
                    });
                }

                if (!string.IsNullOrEmpty(path))
                {
                    options.IncludeXmlComments(path);
                }

                options.DescribeAllEnumsAsStrings();
            });
        }
    }
}
