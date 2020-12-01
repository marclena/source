using System;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain
{
    internal class FromJourneyDTO : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var entity = source as Journey;

            if (entity == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new Vueling.XXX.Library.Entities.Journey
            {
                Arrival = entity.Arrival,
                ArrivalDate = entity.ArrivalDate,
                BookingId = entity.BookingId,
                Departure = entity.Departure,
                DepartureDate = entity.DepartureDate,
                Id = entity.Id,
                Price = entity.Price
            } as TOutput;
        }

    }
}