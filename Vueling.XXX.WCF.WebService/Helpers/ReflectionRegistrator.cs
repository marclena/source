using System.Diagnostics;
using Autofac;
using Vueling.DIRegister.WebService.ServiceLibrary;
using Vueling.XXX.EF.DB.Infrastructure.Configuration;
using TasklingDIRegister = ATC.Taskling.Client.DIRegister.NetFramework.DIRegister;

namespace Vueling.XXX.WCF.WebService.Helpers
{
    public class ReflectionRegistrator : DIWebService
    {
        protected override void CustomDependenciesRegister(ContainerBuilder builder)
        {
            Trace.TraceInformation("Execute override of CustomDependenciesRegister.");

            TasklingDIRegister.RegisterTasklingClientDependencies<TasklingLogger, InfrastructureConfiguration>(builder);
        }

        protected override void ResolveAfterBuildContainer(IContainer container)
        {
            Trace.TraceInformation("Execute override of ResolveAfterBuildContainer.");
        }
    }
}