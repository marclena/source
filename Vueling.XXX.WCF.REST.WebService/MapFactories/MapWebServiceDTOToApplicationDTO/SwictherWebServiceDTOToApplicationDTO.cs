using System;
using System.Globalization;

namespace Vueling.XXX.WCF.REST.WebService.MapFactories.MapWebServiceDTOToApplicationDTO
{
    internal class SwictherWebServiceDTOToApplicationDTO
    {

        private SwictherWebServiceDTOToApplicationDTO()
        {

        }

        internal static MapWebServiceDTOToApplicationDTOFactoryBase GetFactoryFor(EnumApplicationDTO model)
        {
            switch (model)
            {
                case EnumApplicationDTO.FlightDTO:
                    return new ReservationSeatRequestDTOToFlightDTO();
                case EnumApplicationDTO.SeatDTO:
                    return new ReservationSeatRequestDTOToSeatDTO();
                default:
                    throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", model));
            }
        }

    }
}