using System;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain
{
    internal class FromPassengerDTO : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var entity = source as Passenger;

            if (entity == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new Vueling.XXX.Library.Entities.Passenger
            {
                BookingId = entity.BookingId,
                FullName = entity.FullName,
                Id = entity.Id,
                PaxType = (Vueling.XXX.Library.Entities.Passenger.PassengerType)entity.PaxType,
            } as TOutput;
        }

    }
}