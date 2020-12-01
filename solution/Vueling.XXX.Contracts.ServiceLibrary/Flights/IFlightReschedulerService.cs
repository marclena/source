using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.XXX.Contracts.ServiceLibrary.Flights
{
    public interface IFlightReschedulerService
    {
        DateTime RescheduleFlight(string flightIdentifier, DateTime newDepartureDate);
    }
}
