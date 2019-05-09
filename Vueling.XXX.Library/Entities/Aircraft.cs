using System;
using System.Collections.Generic;
using System.Linq;

namespace Vueling.XXX.Library.Entities
{
    public class Aircraft
    {
        public Aircraft(string flightNumber, DateTime departureDate, List<Seat> seats)
        {
            FlightNumber = flightNumber;
            DepartureDate = departureDate;
            Seats = seats;
        }

        public Aircraft(string flightNumber, DateTime departureDate)
        {
            FlightNumber = flightNumber;
            DepartureDate = departureDate;
            Seats = new List<Seat>();
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

        public List<Seat> Seats { get; }

        public IEnumerable<Seat> AvailableSeats
        {
            get { return this.Seats.Where(x => x.IsAvailable); }
        }

        public bool Assign(Seat seat)
        {
            var currentSeat = Seats.FirstOrDefault(x => x.Row == seat.Row && x.Column == seat.Column);
            if (currentSeat == null) 
            { 
                return false; 
            }
            if (currentSeat.IsAvailable) 
            {
                this.Seats.First(x => x.Row == seat.Row && x.Column == seat.Column).Availability = AvailabilityEnum.Busy;
                return true;
            }

            return false;
        }

        public bool Unassign(Seat seat)
        {
            var currentSeat = Seats.FirstOrDefault(x => x.Row == seat.Row && x.Column == seat.Column);
            if (currentSeat.IsNull) 
            { 
                return false; 
            }
            this.Seats.First(x => x.Row == seat.Row && x.Column == seat.Column).Availability = AvailabilityEnum.Available;

            return true;
        }

    }
}
