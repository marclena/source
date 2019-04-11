using System;

namespace Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO
{
    public class PublishedFlightDTO
    {

        public PublishedFlightDTO(string flightNumber, DateTime departureTime)
        {
            Identifier = flightNumber;
            DepartureTime = departureTime;
        }

        public string Identifier { get; set; }

        public DateTime DepartureTime { get; set; }

    }
}
