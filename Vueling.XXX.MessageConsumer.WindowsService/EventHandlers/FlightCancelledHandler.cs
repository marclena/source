using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Vueling.Extensions.Library.DI;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Consumers.Events;
using Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights;
using Vueling.XXX.Contracts.ServiceLibrary.Flights;
using Vueling.XXX.Message.Events.Flights;
using Vueling.XXX.MessageConsumer.WindowsService.MapFactories.MapMessageToApplicationDTO;

namespace Vueling.XXX.MessageConsumer.WindowsService.EventHandlers
{
    [RegisterService]
    public class FlightCancelledHandler : IHandleEvent<FlightCancelled>
    {
        private readonly IUpdateFlightService _updateFlightService;

        public FlightCancelledHandler(IUpdateFlightService updateFlightService)
        {
            _updateFlightService = updateFlightService;
        }

        public void Handle(IEventProxy eventProxy)
        {
            var flightRescheduledEvent = eventProxy.GetEvent<FlightCancelled>();

            try
            {
                var mapper = SwitcherMessageToApplicationDTO.GetFactoryFor(MessageType.FlightCancelled);
                var flightCancelledDTO = mapper.MapToDTO<FlightCancelled, FlightCancelledDTO>(flightRescheduledEvent);
                if (flightCancelledDTO.CancellationReason == "FORCED_TO_FAIL_PLEASE")
                {
                    throw new Exception("MANUALLY FORCED TO FAIL");
                }
                _updateFlightService.UpdateFlightStatus(flightCancelledDTO);

                eventProxy.Completed();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed processing FlightRescheduled event. " + ex);
                eventProxy.Failed(ex);
            }
        }
    }
}
