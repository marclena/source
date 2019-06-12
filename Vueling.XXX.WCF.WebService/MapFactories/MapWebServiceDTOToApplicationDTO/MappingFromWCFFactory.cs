using System;
using Vueling.XXX.WCF.WebService.MapFactories.MapWebServiceDTOToApplicationDTO;

namespace Vueling.XXX.WCF.WebService.MapFactories.ApplicationDTOToMapWebServiceDTO
{
    internal static class MappingFromWCFFactory
    {
        internal static MappingBase GetFactoryFor(EnumApplicationDTO model)
        {
            switch (model)
            {
                case EnumApplicationDTO.FlightDTO:
                    return new ReservationSeatRequestDTOToFlightDTO();
                case EnumApplicationDTO.SeatDTO:
                    return new ReservationSeatRequestDTOToSeatDTO();
                default:
                    throw new NotImplementedException(string.Format("The factory for type {0} is not implemented.", model));
            }
        }

    }
}