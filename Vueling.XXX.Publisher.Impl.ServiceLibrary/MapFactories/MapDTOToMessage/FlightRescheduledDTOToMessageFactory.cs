using System;
using Vueling.XXX.Message.Entities;
using Vueling.XXX.Message.Events.Flights;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage
{
    class FlightRescheduledDTOToMessageFactory : MapDTOToMessageFactoryBase
    {

        internal override TOutput GetMessageFromApplicationDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var flightCreateRequestDTO = webServiceDto as FlightRescheduledDTO;
            if (flightCreateRequestDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage has fail."); }

            var flightRescheduled = new FlightRescheduled()
            {
                Identifier = flightCreateRequestDTO.Identifier,
                NewDepartureTime = flightCreateRequestDTO.NewDepartureTime,
                OldDepartureTime = flightCreateRequestDTO.OldDepartureTime
            };

            return (flightRescheduled) as TOutput;

        }

    }
}
