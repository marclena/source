using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary.Flights;

namespace Vueling.XXX.Impl.ServiceLibrary.Implementations
{
    [RegisterService]
    public class FlightReschedulerService : IFlightReschedulerService
    {
        public DateTime RescheduleFlight(string flightIdentifier, DateTime newDepartureDate)
        {
            // business logic here

            // return the new original departure date
            return newDepartureDate.AddHours(2);
        }
    }
}
