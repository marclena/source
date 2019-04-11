using System.Diagnostics;
using Autofac;
using Vueling.DIRegister.WebService.ServiceLibrary;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Endpoints;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers.Dispatching;

namespace Vueling.XXX.Publisher.WCF.WebService.Helpers
{
    public class ReflectionRegistrator : DIWebService
    {
        protected override void CustomDependenciesRegister(ContainerBuilder builder)
        {
            builder.RegisterType<EndpointResolver>().As<IDependencyResolver>();
            Trace.TraceInformation("Execute override of CustomDependenciesRegister.");
        }

        protected override void ResolveAfterBuildContainer(IContainer container)
        {
            Trace.TraceInformation("Execute override of ResolveAfterBuildContainer.");

            Endpoint.InitializeAsPublisherOnly(container.Resolve<IEndpointManager>());
        }
    }
}