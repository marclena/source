using System;
using System.Collections.Generic;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.EF.DB.Infrastructure.IntegrationTest.MocksFactory
{
    internal class InMemoryBookingProvider
    {
        internal static List<Booking> CreateBatch(int amount)
        {
            var bookings = new List<Booking>();

            var routes = new List<string> { "BCNAMS", "BCNAGP", "FCOCTA", "AGPAMS", "FCOIBZ", "MADPAR" };
            var agents = new List<string> { "Web", "Mobile", "Agency", "Corporate" };

            var randomAgent = new Random();
            var randomRoute = new Random();
            var randomPrice = new Random();

            for (int i = 0; i < amount; i++)
            {
                var agentIndex = randomAgent.Next(4);
                int routeIndex = randomRoute.Next(routes.Count);
                int price = randomPrice.Next(50, 100);

                var newBooking = BuildTestBooking(routes[routeIndex], agents[agentIndex], price);
                bookings.Add(newBooking);
            }

            return bookings;
        }

        private static Booking BuildTestBooking(string route, string agent, int price)
        {
            var newBooking = new Booking
            {
                SalesAgent = agent,
                RecordLocator = Guid.NewGuid().ToString()
            };

            var journey = new Journey
            {
                Arrival = route.Substring(3, 3),
                ArrivalDate = DateTime.Now.AddDays(10),
                Departure = route.Substring(0, 3),
                DepartureDate = DateTime.Today.AddDays(9),
                Price = price
            };

            var passenger = new Passenger
            {
                FullName = "Full Pax Name",
                PaxType = Passenger.PassengerType.ADU
            };

            newBooking.AddJourney(journey);
            newBooking.AddPassenger(passenger);
            return newBooking;
        }
    }
}
