using Autofac;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Vueling.Configuration.Library;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.DTO;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers.Dispatching;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Endpoints;
using Vueling.XXX.MessageConsumer.WindowsService.Bootstrapping.Fluent;
using Vueling.XXX.MessageConsumer.WindowsService.Helpers;

namespace Vueling.XXX.MessageConsumer.WindowsService.Bootstrapping
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1001:GlobalUseDecoratedServices")]
    public class MessageConsumerBuilder : IRegisterCustomisations, IBuildWindowsService
    {
        #region .: Boilerplate (don't change) :.

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        private ContainerBuilder _builder;

        private RegisterDefinition _registerDefinition;
        private ReflectionRegistrator _reflectionRegistrator;
        

        private MessageConsumerBuilder(params Type[] eventHandlers)
        {
            _registerDefinition = new RegisterDefinition()
            {
                IgnoreTypes = new List<Type>()
                {
                    typeof(IDependencyResolver)
                },
                ExecutingAssembly = Assembly.GetExecutingAssembly(),
                AdditionalEntryServices = new List<Type>() {
                    typeof(ConsumerWindowsService),
                    typeof(IEndpointManager)
                },
                DefaultServiceLifetimeScope = LifetimeScopes.InstancePerDependency
            };

            foreach (var eventHandler in eventHandlers)
                _registerDefinition.AdditionalEntryServices.Add(eventHandler);

            _builder = new ContainerBuilder();
            _reflectionRegistrator = new ReflectionRegistrator();
        }
                
        public static IRegisterCustomisations RegisterMessageHandlers(params Type[] eventHandlers)
        {
            return new MessageConsumerBuilder(eventHandlers);
        }

        public ConsumerWindowsService BuildWithVerbose()
        {
            return Build(true);
        }

        public ConsumerWindowsService Build()
        {
            return Build(false);
        }

        private ConsumerWindowsService Build(bool verbose)
        {
            try
            {
                RegisterDependencies(verbose);
                UpdateBuilder(_reflectionRegistrator.Container);
                EndpointResolver.Container = _reflectionRegistrator.Container;
                return _reflectionRegistrator.Container.Resolve<ConsumerWindowsService>();
            }
            catch(Exception ex)
            {
                Trace.TraceError("Could not build windows service: " + ex);
                throw;
            }
        }

        private void RegisterDependencies(bool verbose)
        {
            if (verbose)
                _reflectionRegistrator.EnableVerboseTrace();

            _reflectionRegistrator.RegisterDependencies(_registerDefinition);
        }

        private void UpdateBuilder(IContainer container)
        {
            _builder.Update(container);
        }

        #endregion .: Boilerplate (don't change) :.

        #region .: Add Your Custom DI Code here :.

        public IBuildWindowsService RegisterCustomisations()
        {
            _builder.RegisterType<EndpointResolver>().As<IDependencyResolver>();

            // custom registrations


            // customise RegisterDefinition


            return this;
        }

        #endregion
    }
}
