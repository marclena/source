using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.DB.Infrastructure.UnitTest.Helpers
{
    internal static class AircraftEntityComparerAllFields
    {

        internal static void AssertAreEqual(Aircraft expected, Aircraft actual)
        {

            Assert.IsTrue(actual.AvailableSeats.Count() == expected.AvailableSeats.Count(), "Available seats number not mapped correctly.");

            Assert.IsTrue(expected.FlightNumber == actual.FlightNumber, "Flight number not mapped correctly.");

            Assert.IsTrue(expected.DepartureDate == actual.DepartureDate, "Departure date not mapped correctly.");

        }

    }
}
