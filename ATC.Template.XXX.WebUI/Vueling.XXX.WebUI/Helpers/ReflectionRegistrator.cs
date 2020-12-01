using System.Diagnostics;
using Autofac;
using Vueling.DIRegister.WebUI.ServiceLibrary;

namespace Vueling.XXX.WebUI.Helpers
{
    public class ReflectionRegistrator : DIWebUI
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