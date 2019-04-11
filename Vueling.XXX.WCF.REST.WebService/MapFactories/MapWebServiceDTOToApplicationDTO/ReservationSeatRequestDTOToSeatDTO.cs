using System;
using System.Globalization;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.WCF.REST.WebService.DTO;

namespace Vueling.XXX.WCF.REST.WebService.MapFactories.MapWebServiceDTOToApplicationDTO
{
    class ReservationSeatRequestDTOToSeatDTO : MapWebServiceDTOToApplicationDTOFactoryBase
    {

        internal override TOutput GetApplicationDTOFromWebServiceDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var reservationSeatDTO = webServiceDto as ReservationSeatRequestDTO;
            if (reservationSeatDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.WCF.WebService.DTO.ReservationSeatRequestDTO has fail."); }

            var seatDTO = new SeatDTO(reservationSeatDTO.SeatRow.ToString(CultureInfo.InvariantCulture), reservationSeatDTO.SeatColum);

            return (seatDTO) as TOutput;

        }

    }
}