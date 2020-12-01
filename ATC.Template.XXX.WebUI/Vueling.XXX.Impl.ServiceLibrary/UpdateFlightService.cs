using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights;
using Vueling.XXX.Contracts.ServiceLibrary.Flights;
using Vueling.XXX.Impl.ServiceLibrary.Exceptions;

namespace Vueling.XXX.Impl.ServiceLibrary
{
    [RegisterService]
    public class UpdateFlightService : IUpdateFlightService
    {
        public void UpdateFlightDate(FlightRescheduledDTO flightRescheduledDto)
        {
            ValidateFlightChange(flightRescheduledDto);

            // do some business logic, for example, one of the following:
            // perhaps update the flight in the application's database
            // notify passengers of the change
            // notify crew of the change
        }

        public void UpdateFlightStatus(FlightCancelledDTO flightCancelledDto)
        {
            // do some business logic, for example, one of the following:
            // perhaps update the flight in the application's database
            // notify passengers of the change
            // notify crew of the change
        }

        private void ValidateFlightChange(FlightRescheduledDTO flightRescheduledDto)
        {
            if(flightRescheduledDto.NewDepartureTime == DateTime.MinValue)
            {
                throw new InvalidFlightDetailsException("NewDepartureTime cannot be the year 0000");
            }
        }
    }
}
