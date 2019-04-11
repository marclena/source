using System;

namespace Vueling.XXX.Contracts.ServiceLibrary.DTO
{
    public class FlightDTO
    {

        public FlightDTO(string flightNumber, DateTime departureTime)
        {
            Identifier = flightNumber;
            DepartureTime = departureTime;
        }

        public string Identifier { get; set; }

        public DateTime DepartureTime { get; set; }

    }
}
