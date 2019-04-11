using System.Diagnostics;
using Autofac;
using Vueling.DIRegister.WindowsService.ServiceLibrary;

namespace Vueling.XXX.Subscriber.WindowsService.Helpers
{
    public class ReflectionRegistrator : DIWindowsService
    {
        protected override void CustomDependenciesRegister(ContainerBuilder builder)
        {
            Trace.TraceInformation("Execute override of CustomDependenciesRegister.");
        }
    }
}