using System;
using System.ServiceModel;
using XX.Template.Repository.Contracts;
using XX.Template.Repository.Impl.Configuration.Options;
using XX.Template.Repository.Impl.Context;
using ExternalServiceName.ExternalService.BookingManager;
using ExternalServiceName.ExternalService.SessionManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XX.Template.Repository.Impl.Configuration
{
    public static class LibraryExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<NavitaireParametersOptions>(opt => configuration.Bind("NavitaireParameters", opt));
            services.AddTransient<INavitaireWcfProxy, NavitareWcfProxy>();

            RegisterDbContexts(services, configuration);
            RegisterExternalServices(services, configuration);

            return services;
        }

        private static void RegisterDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<FooDatabaseContext>(options =>
            {
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != null &&
                    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                        .Equals("test", StringComparison.OrdinalIgnoreCase))
                    options.UseInMemoryDatabase("FooDatabaseInMemory");
                else
                    options.UseSqlServer(configuration.GetConnectionString("FooDatabaseConnection"));
            });

            services.AddScoped<IBookingLogRepository, BookingLogRepository>();
        }

        private static void RegisterExternalServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x => new BasicHttpBinding(BasicHttpSecurityMode.Transport)
            {
                MaxReceivedMessageSize = int.MaxValue
            });
            services.AddScoped(x => new ChannelFactory<ISessionManager>(x.GetRequiredService<BasicHttpBinding>(),
                new EndpointAddress($"{configuration["NavitaireParameters:ApiSoapUrl"]}/SessionManager.svc")));
            services.AddTransient(x => x.GetRequiredService<ChannelFactory<ISessionManager>>().CreateChannel());
            services.AddScoped(x => new ChannelFactory<IBookingManager>(x.GetRequiredService<BasicHttpBinding>(),
                new EndpointAddress($"{configuration["NavitaireParameters:ApiSoapUrl"]}/BookingManager.svc")));
            services.AddTransient(x => x.GetRequiredService<ChannelFactory<IBookingManager>>().CreateChannel());
        }
    }
}