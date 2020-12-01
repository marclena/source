using System;
using System.Collections.Generic;
using System.Linq;

namespace Vueling.XXX.Library.Entities
{
    public class Booking
    {
        public Booking()
        {
            //this.Journeys = new HashSet<Journey>();
            //this.Passengers = new HashSet<Passenger>();

            Created = DateTime.UtcNow;
            UpdateModifiedDate();
        }

        public int Id { get; set; }
        public string SalesAgent { get; set; }
        public System.DateTime Created { get; set; }
        public System.DateTime Modified { get; set; }

        string _RecordLocator;
        public string RecordLocator
        {
            get { return _RecordLocator; }
            set
            {
                UpdateModifiedDate();
                _RecordLocator = value;
            }
        }

        public virtual ICollection<Journey> Journeys { get; set; }
        public virtual ICollection<Passenger> Passengers { get; set; }

        public string GetRoute()
        {
            if (Journeys == null || Journeys.Count == 0) { return string.Empty; }

            var firstJourney = Journeys.OrderBy(x => x.DepartureDate).First();
            var lastJourney = Journeys.OrderBy(x => x.DepartureDate).Last();

            return string.Format("{0}{1}", firstJourney.Departure, lastJourney.Arrival);
        }

        public void AddJourney(Journey journey)
        {
            ValidateNewJourney(journey);

            if (Journeys == null) { Journeys = new HashSet<Journey>(); }

            Journeys.Add(journey);

            UpdateModifiedDate();
        }

        public void AddPassenger(Passenger passenger)
        {
            ValidateNewPassenger(passenger);

            if (Passengers == null) { Passengers = new HashSet<Passenger>(); }

            EnsurePaxType(passenger);

            Passengers.Add(passenger);

            UpdateModifiedDate();
        }

        public decimal GetTotalPrice()
        {
            if (!Journeys.Any()) { return default(decimal); }

            return Journeys.Sum(x => x.Price);
        }

        public bool IsAlreadyFlew()
        {
            return Journeys != null && !Journeys.Any(x => x.DepartureDate > DateTime.UtcNow);
        }

        private static void EnsurePaxType(Passenger passenger)
        {
            if (passenger.PaxType == Passenger.PassengerType.Unassigned) { passenger.PaxType = Passenger.PassengerType.ADU; }
        }

        private void ValidateNewJourney(Journey journey)
        {
            if (journey == null) { throw new ArgumentNullException("journey"); }
            if (string.IsNullOrEmpty(journey.Departure) || string.IsNullOrEmpty(journey.Arrival)) { throw new Exception("Paremeters Departure and Arrival are required to add new journey."); }
        }

        private void ValidateNewPassenger(Passenger passenger)
        {
            if (passenger == null) { throw new ArgumentNullException("passenger"); }
            if (string.IsNullOrEmpty(passenger.FullName)) { throw new Exception("Paremeter FullName is required to add new passenger."); }
        }

        private void UpdateModifiedDate()
        {
            Modified = DateTime.UtcNow;
        }

    }
}
