using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Messaging;
using System.ServiceProcess;
using System.Text;
using Vueling.Configuration.Library;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers;
using Vueling.XXX.MessageConsumer.WindowsService.Bootstrapping;
using Vueling.XXX.MessageConsumer.WindowsService.CommandHandlers;
using Vueling.XXX.MessageConsumer.WindowsService.EventHandlers;

namespace Vueling.XXX.MessageConsumer.WindowsService
{
    static class Program
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "args")]
        static void Main(string[] args)
        {
            VuelingEnvironment.InitializeCurrentForApplication("Vueling.XXX.MessageConsumer.WindowsService");

            var service = MessageConsumerBuilder.RegisterMessageHandlers(
                        typeof(FlightRescheduledHandler),
                        typeof(FlightCancelledHandler),
                        typeof(RescheduleFlightHandler))
                    .RegisterCustomisations()
                    .Build();
            Trace.TraceInformation("Service created");

#if (!DEBUG)
            ServiceBase.Run(service);
#else
            service.Start();
#endif
        }


    }
}
