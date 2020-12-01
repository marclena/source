using System;

namespace Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights
{
    public class FlightRescheduledDTO
    {
        public string Identifier { get; set; }
        public DateTime OldDepartureTime { get; set; }
        public DateTime NewDepartureTime { get; set; }
    }
}
