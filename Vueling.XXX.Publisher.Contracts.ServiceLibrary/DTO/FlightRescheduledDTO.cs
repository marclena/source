using System;

namespace Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO
{
    public class FlightRescheduledDTO
    {
        public string Identifier { get; set; }
        public DateTime OldDepartureTime { get; set; }
        public DateTime NewDepartureTime { get; set; }
    }
}
