using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.ToDTOAsIQueryable
{
    internal class IQueryableFromDomainMapFactory
    {
        internal static ToDTOAsIQueryable.IQueryableMappingBase GetFor(DomainToDtoEnum entityName)
        {
            switch (entityName)
            {
                //case DomainToDtoEnum.Booking:
                //    return new FromBookingEntity();
                case DomainToDtoEnum.Journey:
                    return new FromJourneyEntity();
                case DomainToDtoEnum.Passenger:
                    return new FromPassengerEntity();
                default:
                    throw new NotImplementedException(string.Format("Missing mapping for {0} in Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDomainToDTO.", entityName.ToString()));
            }
        }
    }
}
