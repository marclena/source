using System;
using System.Linq;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDomainToDTO
{
    internal class FromBookingEntity : MappingBase
    {
        MappingBase jouneyMapping = MappingFromDomainFactory.GetFor(DomainToDtoEnum.Journey);
        MappingBase paxMapping = MappingFromDomainFactory.GetFor(DomainToDtoEnum.Passenger);

        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var entity = source as Vueling.XXX.Library.Entities.Booking;

            if (entity == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new BookingDTO
            {
                Created = entity.Created,
                Id = entity.Id,
                Journeys = jouneyMapping.GetCollection<Vueling.XXX.Library.Entities.Journey, Journey>(entity.Journeys).ToList(),
                Modified = entity.Modified,
                Passengers = paxMapping.GetCollection<Vueling.XXX.Library.Entities.Passenger, Passenger>(entity.Passengers).ToList(),
                RecordLocator = entity.RecordLocator,
                SalesAgent = entity.SalesAgent,
                TotalPrice = entity.GetTotalPrice()
            } as TOutput;
        }

    }
}