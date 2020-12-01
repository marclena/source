using System;
using System.Globalization;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.WebUI.Models;

namespace Vueling.XXX.WebUI.MapFactories.DataModelToApplicationDTO
{
    class ReservationSeatDataModelToSeatDTO : MapDataModelToApplicationDTOFactoryBase
    {

        internal override TOutput GetApplicationDTOFromWebServiceDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var reservationSeatDTO = webServiceDto as ReservationSeatDataModel;
            if (reservationSeatDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.WebUI.DTO.ReservationSeatDataModel has fail."); }

            var seatDTO = new SeatDTO(reservationSeatDTO.SeatRow.ToString(CultureInfo.InvariantCulture), reservationSeatDTO.SeatColum);

            return (seatDTO) as TOutput;

        }

    }
}