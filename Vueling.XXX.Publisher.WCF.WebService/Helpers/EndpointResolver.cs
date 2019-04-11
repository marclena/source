using Autofac;
using System;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers.Dispatching;
using Vueling.Extensions.Library.DI;

namespace Vueling.XXX.Publisher.WCF.WebService.Helpers
{
    /// <summary>
    /// This class allows the Endpoint to resolve application services used by event/command handlers
    /// </summary>
    [RegisterService]
    public class EndpointResolver : IDependencyResolver
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        public static IContainer Container;

        public dynamic Resolve(Type type)
        {
            return Container.Resolve(type);
        }
    }
}
