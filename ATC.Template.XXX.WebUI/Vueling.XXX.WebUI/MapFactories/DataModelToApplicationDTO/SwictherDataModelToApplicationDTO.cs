using System;
using System.Globalization;

namespace Vueling.XXX.WebUI.MapFactories.DataModelToApplicationDTO
{
    internal class SwictherDataModelToApplicationDTO
    {

        private SwictherDataModelToApplicationDTO()
        {

        }

        internal static MapDataModelToApplicationDTOFactoryBase GetFactoryFor(EnumApplicationDTO model)
        {
            switch (model)
            {
                case EnumApplicationDTO.FlightDTO:
                    return new ReservationSeatDataModelToFlightDTO();
                case EnumApplicationDTO.SeatDTO:
                    return new ReservationSeatDataModelToSeatDTO();
                default:
                    throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", model));
            }
        }

    }
}