using System;
using System.Collections.Generic;
using System.Linq;

namespace Vueling.XXX.Library.Entities
{
    public class Aircraft
    {

        private List<Seat> _seats;

        public Aircraft(string flightNumber, DateTime departureDate, List<Seat> seats)
        {
            FlightNumber = flightNumber;
            DepartureDate = departureDate;
            _seats = seats;
        }

        public Aircraft(string flightNumber, DateTime departureDate)
        {
            FlightNumber = flightNumber;
            DepartureDate = departureDate;
            _seats = new List<Seat>();
        }


        public string FlightNumber { 
            get; 
            protected set; 
        }

        public DateTime DepartureDate 
        { 
            get; 
            protected set; 
        }

        public List<Seat> Seats 
        {
            get { return _seats; } 
        }

        public IEnumerable<Seat> AvailableSeats
        {
            get { return this.Seats.Where(x => x.IsAvailable); }
        }

        public bool Assign(Seat seat)
        {
            var currentSeat = Seats.Where(x => x.Row == seat.Row && x.Column == seat.Column).FirstOrDefault();
            if (currentSeat == null) 
            { 
                return false; 
            }
            if (currentSeat.IsAvailable) 
            {
                this.Seats.Where(x => x.Row == seat.Row && x.Column == seat.Column).FirstOrDefault().Availability = AvailabilityEnum.Busy;
                return true;
            }

            return false;
        }

        public bool Unassign(Seat seat)
        {
            var currentSeat = Seats.Where(x => x.Row == seat.Row && x.Column == seat.Column).FirstOrDefault();
            if (currentSeat.IsNull) 
            { 
                return false; 
            }
            this.Seats.Where(x => x.Row == seat.Row && x.Column == seat.Column).FirstOrDefault().Availability = AvailabilityEnum.Available;

            return true;
        }

    }
}
