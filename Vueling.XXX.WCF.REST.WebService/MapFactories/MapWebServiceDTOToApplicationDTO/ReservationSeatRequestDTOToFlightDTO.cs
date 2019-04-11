using System;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.WCF.REST.WebService.DTO;

namespace Vueling.XXX.WCF.REST.WebService.MapFactories.MapWebServiceDTOToApplicationDTO
{
    class ReservationSeatRequestDTOToFlightDTO : MapWebServiceDTOToApplicationDTOFactoryBase
    {

        internal override TOutput GetApplicationDTOFromWebServiceDTO<TInput, TOutput>(TInput webServiceDto)
        {

            var reservationSeatDTO = webServiceDto as ReservationSeatRequestDTO;
            if (reservationSeatDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.WCF.REST.WebService.DTO.ReservationSeatRequestDTOToFlightDTO has fail."); }

            var flightDTO = new FlightDTO(reservationSeatDTO.FlighIdentifier, reservationSeatDTO.DepartureTime);

            return (flightDTO) as TOutput;

        }

    }
}