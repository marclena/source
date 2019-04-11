using System;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDomainToDTO
{
    internal class FromPassengerEntity : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var entity = source as Vueling.XXX.Library.Entities.Passenger;

            if (entity == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new Passenger
            {
                BookingId = entity.BookingId,
                FullName = entity.FullName,
                Id = entity.Id,
                PaxType = (Passenger.PassengerType)entity.PaxType,
            } as TOutput;
        }

    }
}