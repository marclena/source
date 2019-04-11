using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights;
using Vueling.XXX.Message.Events.Flights;

namespace Vueling.XXX.MessageConsumer.WindowsService.MapFactories.MapMessageToApplicationDTO
{
    class FlightRescheduledMapper : MapMessageToApplicationDTOFactoryBase
    {

        internal override TOutput MapToDTO<TInput, TOutput>(TInput message)
        {

            var flightRescheduled = message as FlightRescheduled;
            if (flightRescheduled == null) { throw new InvalidCastException("Cast to type FlightRescheduled has failed."); }

            var flightRescheduledDTO = new FlightRescheduledDTO()
            {
                Identifier = flightRescheduled.Identifier,
                OldDepartureTime = flightRescheduled.OldDepartureTime,
                NewDepartureTime = flightRescheduled.NewDepartureTime
            };

            return (flightRescheduledDTO) as TOutput;
        }

    }
}
