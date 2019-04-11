using System;

namespace Vueling.XXX.Message.Entities
{
    public class FlightMessageDTO
    {

        public FlightMessageDTO()
        {

        }

        public FlightMessageDTO(string flightNumber, DateTime departureTime)
        {
            Identifier = flightNumber;
            DepartureTime = departureTime;
        }

        public string Identifier { get; set; }

        public DateTime DepartureTime { get; set; }

    }
}
