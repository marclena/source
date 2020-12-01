using System.Collections.Generic;
using XX.Template.Library.Contracts;
using XX.Template.Library.Impl.Error;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XX.Template.Library.Impl.Configuration
{
    public static class LibraryExtension
    {
        public static IServiceCollection AddLibraryServices(this IServiceCollection services, 
                                                IConfiguration configuration)
        {       
            services.AddScoped<IFooService, FooService>();
            services.AddTransient<IGoogleService, GoogleService>();
            services.Configure<Dictionary<BusinessErrorType, BusinessErrorObject>>(opt =>
                configuration.Bind("BusinessErrors", opt));
            return services;
        }
    }
}