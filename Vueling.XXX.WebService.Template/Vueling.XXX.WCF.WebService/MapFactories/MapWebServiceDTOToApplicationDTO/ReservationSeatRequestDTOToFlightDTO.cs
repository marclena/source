using System;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.WCF.WebService.DTO;

namespace Vueling.XXX.WCF.WebService.MapFactories.MapWebServiceDTOToApplicationDTO
{
    class ReservationSeatRequestDTOToFlightDTO : MappingBase
    {

        internal override TOutput Get<TInput, TOutput>(TInput webServiceDto)
        {

            var reservationSeatDTO = webServiceDto as ReservationSeatRequestDTO;
            if (reservationSeatDTO == null) { throw new InvalidCastException("Cast to type Vueling.XXX.WCF.WebService.DTO.ReservationSeatRequestDTOToFlightDTO has fail."); }

            var flightDTO = new FlightDTO(reservationSeatDTO.FlighIdentifier, reservationSeatDTO.DepartureTime);

            return (flightDTO) as TOutput;

        }

    }
}