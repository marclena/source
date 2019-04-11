using System;
using System.Diagnostics;
using Vueling.Extensions.Library.DI;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Publishers.Common.Tracking;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Publishers.Events;
using Vueling.XXX.Message.Events.Flights;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary
{
    [RegisterService]
    public class FlightEventPublisherService : IFlightEventPublisherService
    {
        private readonly IEventPublisher _eventPublisher;
        
        public FlightEventPublisherService(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public bool PublishFlightRescheduled(FlightRescheduledDTO flightRescheduledDto)
        {
            var mapper = SwitcherDTOToEntityFactory.GetFactoryFor(EnumMessage.FlightRescheduled);
            var flightRescheduled = mapper.GetMessageFromApplicationDTO<FlightRescheduledDTO, FlightRescheduled>(flightRescheduledDto);

            var result = _eventPublisher.PublishEvent(flightRescheduled);
            if (result.Status != SendStatus.Success)
            {
                Trace.TraceError(string.Format("Flight rescheduled event not sent. Identifier {0}. Status: {1}, Description: {2}",
                    flightRescheduled.Identifier, result.Status, result.Description));
                return false;
            }

            return true;
        }

        public bool PublishFlightCancelled(FlightCancelledDTO flightCancelledDto)
        {
            var mapper = SwitcherDTOToEntityFactory.GetFactoryFor(EnumMessage.FlightCancelled);
            var flightCancelled = mapper.GetMessageFromApplicationDTO<FlightCancelledDTO, FlightCancelled>(flightCancelledDto);

            var result = _eventPublisher.PublishEvent(flightCancelled);
            if (result.Status != SendStatus.Success)
            {
                Trace.TraceError(string.Format("Flight cancelled event not sent. Identifier {0} Status: {1}, Description: {2}",
                    flightCancelled.Identifier, result.Status, result.Description));
                return false;
            }

            return true;
        }
    }
}
