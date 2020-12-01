using System;
using System.Runtime.CompilerServices;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Library.Entities;

[assembly: InternalsVisibleTo("Vueling.XXX.Impl.ServiceLibrary.UnitTest")]
namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain
{
    class SeatDTOToSeatEntityFactory : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput dto)
        {
            var seatDTO = dto as SeatDTO;
            if (seatDTO == null) { throw new InvalidCastException(typeof(TInput).Name); }

            var seat = new Seat();
            seat.Column = seatDTO.Column;
            seat.Row = seatDTO.Row;

            return (seat) as TOutput;
        }

    }
}
