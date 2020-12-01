using ATC.Extensions.Http.Extensions;
using ATC.Extensions.Http.Polly.Extensions;
using XX.Template.Repository.Contracts;
using XX.Template.Repository.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XX.Template.WebApi.Extensions.ServiceCollectionExtensions
{
    public static class ServiceCollectionHttpClientExtensions
    {
        public static void AddCustomHttpClients(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.InitializeHttpClientFactory(configuration);
            services.InitializePollyPolicies(configuration);

            services
                .RegisterHttpClient<IGoogleProxy, GoogleProxy>()
                .AddPoliciesFromRegistry<GoogleProxy>(configuration);
        }
    }
}
