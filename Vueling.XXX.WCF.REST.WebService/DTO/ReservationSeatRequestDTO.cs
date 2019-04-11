using System;

namespace Vueling.XXX.WCF.REST.WebService.DTO
{
    public class ReservationSeatRequestDTO
    {

        public ReservationSeatRequestDTO(string flightIdentifier, DateTime departureTime, int seatRow, string seatColum)
        {
            FlighIdentifier = flightIdentifier;
            DepartureTime = departureTime;
            SeatRow = seatRow;
            SeatColum = seatColum;
        }

        public string FlighIdentifier { get; set; }

        public DateTime DepartureTime { get; set; }

        public int SeatRow { get; set; }

        public string SeatColum { get; set; }

    }
}