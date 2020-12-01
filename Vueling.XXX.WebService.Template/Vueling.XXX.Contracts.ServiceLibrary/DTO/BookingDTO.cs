using System.Collections.Generic;

namespace Vueling.XXX.Contracts.ServiceLibrary.DTO
{
    public class BookingDTO
    {
        //public BookingDTO()
        //{
        //    Journeys = new List<JourneyDTO>();
        //    Passengers = new List<PassengerDTO>();
        //}

        public int Id { get; set; }
        public string SalesAgent { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }
        public string RecordLocator { get; set; }

        public decimal TotalPrice { get; set; }

        public IEnumerable<Journey> Journeys { get; set; }
        public IEnumerable<Passenger> Passengers { get; set; }
    }
}
