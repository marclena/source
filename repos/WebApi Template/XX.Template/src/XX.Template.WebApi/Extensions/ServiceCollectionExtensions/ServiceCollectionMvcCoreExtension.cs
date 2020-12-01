using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionMvcCoreExtension
    {
        public static IServiceCollection AddCustomMvcCore(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddHttpContextAccessor();
            services.AddCustomApiVersioning()
                    .AddMvcCore()
                    .AddVersionedApiExplorer(options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;
                    })
                    .AddAuthorization()
                    .AddApiExplorer()
                    .AddJsonFormatters()
                    .AddJsonOptions(j => j.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None)
                    .AddJsonOptions(j => j.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                    .AddJsonOptions(j => j.SerializerSettings.NullValueHandling = NullValueHandling.Ignore)
                    .AddJsonOptions(j => j.SerializerSettings.Converters.Add(new StringEnumConverter()));
            
            return services;
        }
        
    }
}