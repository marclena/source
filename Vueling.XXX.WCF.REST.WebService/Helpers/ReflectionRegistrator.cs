using System.Diagnostics;
using Autofac;
using Vueling.DIRegister.WebService.ServiceLibrary;

namespace Vueling.XXX.WCF.REST.WebService.Helpers
{
    public class ReflectionRegistrator : DIWebService
    {
        protected override void CustomDependenciesRegister(ContainerBuilder builder)
        {
            Trace.TraceInformation("Execute override of CustomDependenciesRegister.");
        }

        protected override void ResolveAfterBuildContainer(IContainer container)
        {
            Trace.TraceInformation("Execute override of ResolveAfterBuildContainer.");
        }
    }
}