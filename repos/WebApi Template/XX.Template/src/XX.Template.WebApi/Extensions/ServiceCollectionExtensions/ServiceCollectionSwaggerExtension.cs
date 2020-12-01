using ATC.Swagger.Extension;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.Swagger;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionSwaggerExtension
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddCustomSwaggerGen(new ApiKeyScheme
            {
                Type = "apiKey",
                Description = "An authorization header using the bearer scheme",
                Name = "Authorization",
                In = "header"
            });
            return services;
        }
    }
}