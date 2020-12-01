using System;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.WCF.WebService.DTO;

namespace Vueling.XXX.WCF.WebService.MapFactories.MapWebServiceDTOToApplicationDTO
{
    class ReservationSeatRequestDTOToSeatDTO : MappingBase
    {

        internal override TOutput Get<TInput, TOutput>(TInput webServiceDto)
        {

            var reservationSeatDTO = webServiceDto as ReservationSeatRequestDTO;
            if (reservationSeatDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.WCF.WebService.DTO.ReservationSeatRequestDTO has fail."); }

            var seatDTO = new SeatDTO(reservationSeatDTO.SeatRow.ToString(), reservationSeatDTO.SeatColum);

            return (seatDTO) as TOutput;

        }

    }
}