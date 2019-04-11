using System;
using Vueling.XXX.Message.Entities;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage
{
    class FlightDTOToMessageFactory : MapDTOToMessageFactoryBase
    {

        internal override TOutput GetMessageFromApplicationDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var flightCreateRequestDTO = webServiceDto as PublishedFlightDTO;
            if (flightCreateRequestDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage has fail."); }

            var flight = new FlightMessageDTO(flightCreateRequestDTO.Identifier, flightCreateRequestDTO.DepartureTime);

            return (flight) as TOutput;

        }

    }
}
