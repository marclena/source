using System;
using System.Diagnostics;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;

[assembly: CLSCompliant(false)]
namespace Vueling.XXX.Publisher.WCF.WebService
{
    [RegisterService]
    public class FlightEventPublisherWebService : IFlightEventPublisherWebService
    {
        private readonly IFlightEventPublisherService _flightEventPublisherService;

        public FlightEventPublisherWebService(IFlightEventPublisherService flightEventPublisherService)
        {
            _flightEventPublisherService = flightEventPublisherService;
        }

        public bool PublishFlightCancelled(string flightIdentifier, DateTime cancelledDate, string cancellationReason, string cancelledBy)
        {
            try
            {
                var flightCancelled = new FlightCancelledDTO()
                {
                    Identifier = flightIdentifier,
                    CancelledDate = cancelledDate,
                    CancellationReason = cancellationReason,
                    CancelledBy = cancelledBy
                };

                return _flightEventPublisherService.PublishFlightCancelled(flightCancelled);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return false;
            }
        }
    }
}
