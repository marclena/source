using System;
using System.Diagnostics;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary;

namespace Vueling.XXX.Publisher.WCF.WebService
{
    [RegisterService]
    public class RescheduleFlightWebService : IRescheduleFlightWebService
    {
        private readonly IRescheduleFlightPublisherService _flightReschedulerService;

        public RescheduleFlightWebService(IRescheduleFlightPublisherService flightEventPublisherService)
        {
            _flightReschedulerService = flightEventPublisherService;
        }

        public bool RescheduleFlight(string flightIdentifier, DateTime newDepartureTime)
        {
            try
            {
                return _flightReschedulerService.SendRescheduleFlightCommand(flightIdentifier, newDepartureTime);
            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.ToString());
                return false;
            }
        }
        
    }
}
