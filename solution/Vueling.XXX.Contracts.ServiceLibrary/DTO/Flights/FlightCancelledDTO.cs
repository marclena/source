using System;

namespace Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights
{
    public class FlightCancelledDTO
    {
        public string Identifier { get; set; }
        public DateTime DepartureTime { get; set; }
        public string CancellationReason { get; set; }
        public string CancelledBy { get; set; }
    }
}
