//using Autofac;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using Vueling.Configuration.Library;
//using Vueling.RegisterCommonDI.ServiceLibrary;
//using Vueling.XXX.WCF.REST.WebService.IntegrationTest.Helpers;

//namespace Vueling.XXX.WCF.REST.WebService.IntegrationTest
//{
//    internal class ServiceLocator
//    {
//        const string applicationId = "Vueling.XXX.WCF.REST.WebService.IntegrationTest";
//        static IContainer container;

//        static ServiceLocator()
//        {
//            if (!VuelingEnvironment.IsInitialized)
//            {
//                VuelingEnvironment.InitializeCurrentForApplication(applicationId);
//            }

//            var registerDefinition = new RegisterDefinition
//            {
//                ExecutingAssembly = Assembly.Load("Vueling.XXX.WCF.REST.WebService"),
//                ApplyConfigurationAttribute = true,
//                ApplyContextAttribute = true,
//                IgnoreInterfaces = new List<Type>(),
//                ApplyStatefulServiceAttribute = false,
//                DefaultServiceLifetimeScope = RegisterDefinition.DefaultServiceLifetimeScopeEnum.InstancePerLifetimeScope,
//                AdditionalEntryServices = new List<Type>
//                {
//                    typeof(Vueling.XXX.Impl.ServiceLibrary.AircraftMaintenanceApplicationService),
//                    typeof(SeatReservationForAircraftsWebService)
//                }
//            };

//            var registrator = new ReflectionRegistrator();
//            //registrator.EnableVerboseTrace();
//            registrator.RegisterDependencies(registerDefinition);

//            container = registrator.Container;
//        }

//        internal static T Resolve<T>()
//        {
//            return container.Resolve<T>();
//        }
//    }
//}
