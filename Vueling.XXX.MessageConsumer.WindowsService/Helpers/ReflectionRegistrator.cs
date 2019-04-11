using System.Diagnostics;
using Autofac;
using Vueling.DIRegister.Custom.ServiceLibrary;
using Autofac.Core;

namespace Vueling.XXX.MessageConsumer.WindowsService.Helpers
{
    public class ReflectionRegistrator : DICustom
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        public IContainer Container;

        protected override void CustomDependenciesRegister(ContainerBuilder builder)
        {
            Trace.TraceInformation("Execute override of CustomDependenciesRegister.");
        }

        protected override void ResolveAfterBuildContainer(IContainer container)
        {
            Container = container;
        }
    }
}