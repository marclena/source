using System;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.WebUI.Models;

namespace Vueling.XXX.WebUI.MapFactories.DataModelToApplicationDTO
{
    class ReservationSeatDataModelToFlightDTO : MapDataModelToApplicationDTOFactoryBase
    {

        internal override TOutput GetApplicationDTOFromWebServiceDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var reservationSeatDTO = webServiceDto as ReservationSeatDataModel;
            if (reservationSeatDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.WebUI.DTO.ReservationSeatDataModelToFlightDTO has fail."); }

            var flightDTO = new FlightDTO(reservationSeatDTO.FlighIdentifier, reservationSeatDTO.DepartureTime);

            return (flightDTO) as TOutput;

        }

    }
}