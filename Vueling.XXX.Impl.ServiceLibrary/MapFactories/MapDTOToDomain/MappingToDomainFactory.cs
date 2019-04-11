using System;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain
{
    internal class MappingToDomainFactory
    {
        internal static MappingBase GetFor(EnumDomain model)
        {
            switch (model)
            {
                case EnumDomain.Seat:
                    return new SeatDTOToSeatEntityFactory();
                case EnumDomain.Booking:
                    return new FromBookingDTO();
                case EnumDomain.Journey:
                    return new FromJourneyDTO();
                case EnumDomain.Passenger:
                    return new FromPassengerDTO();
                default:
                    throw new NotImplementedException(string.Format("The mapping for type {0} is not implemented.", model));
            }
        }
    }
}
