using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Impl.ServiceLibrary.UnitTest.Helpers
{
    internal static class SeatEntityComparerAllFileds
    {

        internal static void AssertAreEqual(Seat expected, Seat actual)
        {
            Assert.AreEqual(expected.Row, actual.Row, "Row is no mapped correct.");

            Assert.AreEqual(expected.Column, actual.Column, "Colum is no mapped correct.");

        }

    }
}
