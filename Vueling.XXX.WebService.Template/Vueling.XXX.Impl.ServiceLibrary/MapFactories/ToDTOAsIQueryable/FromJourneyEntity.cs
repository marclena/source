using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.ToDTOAsIQueryable
{
    internal class FromJourneyEntity : IQueryableMappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var entity = source as Vueling.XXX.Library.Entities.Journey;

            if (entity == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new Journey
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