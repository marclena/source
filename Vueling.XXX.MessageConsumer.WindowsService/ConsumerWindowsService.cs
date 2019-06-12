using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Vueling.Configuration.Library;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Policies;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers.Dispatching;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Endpoints;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Policies;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Policies.Detectors.BuiltIn;
using Vueling.XXX.Message.Commands;
using Vueling.XXX.Message.Events.Flights;

namespace Vueling.XXX.MessageConsumer.WindowsService
{
    public partial class ConsumerWindowsService : ServiceBase
    {
        private readonly IEndpointManager _endpointManager;

        public ConsumerWindowsService(IEndpointManager endpointManager)
        {
            _endpointManager = endpointManager;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                FailPolicy.CreateFor(typeof(FlightRescheduled), typeof(FlightCancelled), typeof(RescheduleFlight))
                    .ClassifyAsTransient(new SqlTransientErrorDetector())
                    .OnTransientError(retryLimit: 3,
                        retryPeriod: TimeSpan.FromMinutes(1),
                        periodType: PeriodType.ExponentialBackoff,
                        exponent: 2,
                        retryLimitAction: FailAction.SendToFailExchange)
                    .OnPersistentError(FailAction.SendToFailExchange);

                Endpoint.InitializeAsConsumer(_endpointManager)
                    .HandleEvent<FlightRescheduled>()
                    .AndHandleEvent<FlightCancelled>()
                    .AndHandleCommand<RescheduleFlight>()
                    .WithSingleConsumer(perConsumerConcurrency: 1)
                    .Start();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Could not start windows service: " + ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                Endpoint.Stop(TimeSpan.FromSeconds(60));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Could not stop windows service: " + ex);
            }
        }

  
        public void Start()
        {
            OnStart(null);

            Console.WriteLine("Started. Press any key to shutdown the consumer(s)");
            Console.Read();
            Console.WriteLine("Shutting down, this may take a few seconds...");
            Endpoint.Stop(TimeSpan.FromSeconds(60));
        }
    }
}
