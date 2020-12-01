using System;
using System.Linq;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain
{
    internal class FromBookingDTO : MappingBase
    {
        MappingBase jouneyMapping = MappingToDomainFactory.GetFor(EnumDomain.Journey);
        MappingBase paxMapping = MappingToDomainFactory.GetFor(EnumDomain.Passenger);

        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var dto = source as BookingDTO;

            if (dto == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new Vueling.XXX.Library.Entities.Booking
            {
                Created = dto.Created,
                Id = dto.Id,
                Journeys = jouneyMapping.GetCollection<Journey, Vueling.XXX.Library.Entities.Journey>(dto.Journeys).ToList(),
                Modified = dto.Modified,
                Passengers = paxMapping.GetCollection<Passenger, Vueling.XXX.Library.Entities.Passenger>(dto.Passengers).ToList(),
                RecordLocator = dto.RecordLocator,
                SalesAgent = dto.SalesAgent
            } as TOutput;
        }

    }
}