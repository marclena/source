using System;
using System.Diagnostics;
using ATC.Taskling.Client.Contracts.ServiceLibrary;
using ATC.Taskling.Client.Core.Extensions.Logger;
using ATC.Taskling.Client.Core.Extensions.Tasks;
using ATC.Taskling.Client.DIRegister.NetFramework;
using ATC.Taskling.Client.Impl.ServiceLibrary;
using ATC.Taskling.Client.Impl.ServiceLibrary.Blocks;
using ATC.Taskling.Client.Impl.ServiceLibrary.ExecutionContext;
using ATC.Taskling.Client.Repository.Contracts;
using ATC.Taskling.Client.Repository.Contracts.Blocks;
using ATC.Taskling.Client.Repository.Contracts.CleanUp;
using ATC.Taskling.Client.Repository.Contracts.CriticalSections;
using ATC.Taskling.Client.Repository.Contracts.TaskExecution;
using ATC.Taskling.Client.Repository.Impl.AncilliaryServices;
using ATC.Taskling.Client.Repository.Impl.Blocks;
using ATC.Taskling.Client.Repository.Impl.CriticalSections;
using ATC.Taskling.Client.Repository.Impl.TaskExecution;
using ATC.Taskling.Client.Repository.Impl.Tasks;
using ATC.Taskling.Client.Repository.Impl.Tokens;
using ATC.Taskling.Client.Repository.Impl.Tokens.Executions;
using Autofac;
using Vueling.DIRegister.WebService.ServiceLibrary;
using Vueling.XXX.DB.Infrastructure.Configuration;
//using TasklingDIRegister = ATC.Taskling.Client.DIRegister.NetFramework.DIRegister;

namespace Vueling.XXX.WCF.WebService.Helpers
{
    public class ReflectionRegistrator : DIWebService
    {
        protected override void CustomDependenciesRegister(ContainerBuilder builder)
        {
            Trace.TraceInformation("Execute override of CustomDependenciesRegister.");

            //TasklingDIRegister.RegisterTasklingClientDependencies<TasklingLogger, XXXInfrastructureConfiguration>(builder);

            RegisterTasklingClientDependencies<TasklingLogger, XXXInfrastructureConfiguration>(builder);
        }

        protected override void ResolveAfterBuildContainer(IContainer container)
        {
            Trace.TraceInformation("Execute override of ResolveAfterBuildContainer.");
        }

        public void RegisterTasklingClientDependencies<TLogger, TDatabaseConfig>(ContainerBuilder builder) 
            where TLogger : ITasklingEventLogger
            where TDatabaseConfig : IDatabaseConfiguration
        {
            // TASKLING INTERNAL DEPENDENCIES

            builder.RegisterType<CleanUpRepository>().As<ICleanUpRepository>();
            builder.RegisterType<BlockRepository>().As<IBlockRepository>();
            builder.RegisterType<CommonTokenRepository>().As<ICommonTokenRepository>();
            builder.RegisterType<DefaultTaskLogger>().As<ITaskLogger>();
            builder.RegisterType<ExecutionTokenRepository>().As<IExecutionTokenRepository>();
            builder.RegisterType<TaskRepository>().As<ITaskRepository>();
            builder.RegisterType<CleanUpService>().As<ICleanUpService>();
            builder.RegisterType<ObjectBlockRepository>().As<IObjectBlockRepository>();
            builder.RegisterType<ListBlockRepository>().As<IListBlockRepository>();
            builder.RegisterType<RangeBlockRepository>().As<IRangeBlockRepository>();
            builder.RegisterType<BlockFactory>().As<IBlockFactory>();
            builder.RegisterType<CriticalSectionRepository>().As<ICriticalSectionRepository>();
            builder.RegisterType<TaskExecutionRepository>().As<ITaskExecutionRepository>();
            builder.RegisterType<TasklingClient>().As<ITasklingClient>();

            // TASKLING INTERNAL DEPENDENCIES

            // TASKLING EXTERNAL DEPENDENCIES

            builder.RegisterType<TLogger>().As<ITasklingEventLogger>();
            builder.RegisterType<TDatabaseConfig>().As<IDatabaseConfiguration>();
            
            builder.Register(x => 
                        new DbOperationsService(
                            x.Resolve<ITasklingEventLogger>(), 
                            new ClientConnectionSettings(
                                x.Resolve<IDatabaseConfiguration>().ConnectionString,
                                new TimeSpan(0, 0, x.Resolve<IDatabaseConfiguration>().DatabaseTimeout))))
                   .As<IDbOperationsService>();

            // TASKLING EXTERNAL DEPENDENCIES
        }
    }
}