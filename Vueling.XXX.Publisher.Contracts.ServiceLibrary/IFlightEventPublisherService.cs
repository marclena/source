using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Publisher.Contracts.ServiceLibrary
{
    public interface IFlightEventPublisherService
    {
        bool PublishFlightRescheduled(FlightRescheduledDTO flightRescheduled);
        bool PublishFlightCancelled(FlightCancelledDTO flightCancelled);
    }
}
