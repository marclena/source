using System;

namespace Vueling.XXX.WCF.WebService.MapFactories.ApplicationDTOToMapWebServiceDTO
{
    internal static class MappingToWCFFactory
    {
        internal static MappingBase GetFor(DtoToWCFEnum entityName)
        {
            switch (entityName)
            {
                case DtoToWCFEnum.Booking:
                    return new BookingDtoToWCF();
                default:
                    throw new NotImplementedException(string.Format("Missing mapping for {0} in Vueling.XXX.WCF.WebService.MapFactories.ApplicationDTOToMapWebServiceDTO.", entityName.ToString()));
            }
        }
    }
}
