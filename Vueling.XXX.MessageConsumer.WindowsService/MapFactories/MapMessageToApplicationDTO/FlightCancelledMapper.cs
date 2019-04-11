using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.XXX.Contracts.ServiceLibrary.DTO.Flights;
using Vueling.XXX.Message.Events.Flights;

namespace Vueling.XXX.MessageConsumer.WindowsService.MapFactories.MapMessageToApplicationDTO
{
    class FlightCancelledMapper : MapMessageToApplicationDTOFactoryBase
    {

        internal override TOutput MapToDTO<TInput, TOutput>(TInput message)
        {

            var flightCancelled = message as FlightCancelled;
            if (flightCancelled == null) { throw new InvalidCastException("Cast to type FlightRescheduled has failed."); }

            var flightCancelledDTO = new FlightCancelledDTO()
            {
                Identifier = flightCancelled.Identifier,
                CancellationReason = flightCancelled.CancellationReason,
                CancelledBy = flightCancelled.CancelledBy
            };

            return (flightCancelledDTO) as TOutput;
        }

    }
}
