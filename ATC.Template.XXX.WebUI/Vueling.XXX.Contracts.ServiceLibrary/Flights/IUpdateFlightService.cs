using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights;

namespace Vueling.XXX.Contracts.ServiceLibrary.Flights
{
    public interface IUpdateFlightService
    {
        void UpdateFlightDate(FlightRescheduledDTO flightRescheduleDto);
        void UpdateFlightStatus(FlightCancelledDTO flightCancelledDto);
    }
}
