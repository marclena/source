using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vueling.XXX.DB.Infrastructure.UnitTest.Helpers
{
    internal static class AircraftDBObjectComparerAllFileds
    {

        internal static void AssertAreEqual(Infrastructure.Repositories.AircraftRepository.Aircraft expected, Infrastructure.Repositories.AircraftRepository.Aircraft actual)
        {

            Assert.AreEqual(actual.FlightNumber, expected.FlightNumber, "Flight number not mapped.");

            Assert.AreEqual(actual.DepartureDate, expected.DepartureDate, "Departure date not mapped.");

            Assert.IsTrue(actual.BusySeats.Length == expected.BusySeats.Length, "Se esperaba que la cantidad de asientos ocupados fuera la misma después del mapeo.");

        }

    }
}
