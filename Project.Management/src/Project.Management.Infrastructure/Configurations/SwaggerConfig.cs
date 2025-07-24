using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Project.Management.Infrastructure.Configurations
{
    public static class SwaggerConfig
    {
        private static readonly List<string> _xmlFiles =
        [
            "Project.Management.Api",
            "Project.Management.Domain"
        ];

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Project Management API",
                    Description = "API Documentation for Project Management Application",
                });

                // Add comments from XML documentation
                foreach (var file in _xmlFiles)
                {
                    var xmlFile = $"{file}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                    if (File.Exists(xmlPath))
                    {
                        options.IncludeXmlComments(xmlPath);
                    }
                }
            });

            return services;
        }
    }
}
