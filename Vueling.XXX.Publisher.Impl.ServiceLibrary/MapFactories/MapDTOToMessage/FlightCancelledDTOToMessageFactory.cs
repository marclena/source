using System;
using Vueling.XXX.Message.Events.Flights;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage
{
    class FlightCancelledDTOToMessageFactory : MapDTOToMessageFactoryBase
    {

        internal override TOutput GetMessageFromApplicationDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var dto = webServiceDto as FlightCancelledDTO;
            if (dto == null) { throw new InvalidCastException("Cannot cast to type: " + typeof(FlightCancelledDTO).FullName); }

            var flightCancelled = new FlightCancelled()
            {
                Identifier = dto.Identifier,
                CancellationReason = dto.CancellationReason,
                CancelledBy = dto.CancelledBy
            };

            return (flightCancelled) as TOutput;

        }

    }
}
