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
    public class FlightRescheduledHandler : IHandleEvent<FlightRescheduled>
    {
        private readonly IUpdateFlightService _updateFlightService;

        public FlightRescheduledHandler(IUpdateFlightService updateFlightService)
        {
            _updateFlightService = updateFlightService;
        }

        public void Handle(IEventProxy eventProxy)
        {
            var flightRescheduledEvent = eventProxy.GetEvent<FlightRescheduled>();

            try
            {
                var mapper = SwitcherMessageToApplicationDTO.GetFactoryFor(MessageType.FlightRescheduled);
                var flightRescheduledDTO = mapper.MapToDTO<FlightRescheduled, FlightRescheduledDTO>(flightRescheduledEvent);
                _updateFlightService.UpdateFlightDate(flightRescheduledDTO);

                eventProxy.Completed();
            }
            catch(Exception ex)
            {
                Trace.TraceError("Failed processing FlightRescheduled event. " + ex);
                eventProxy.Failed(ex);
            }
        }
    }
}
