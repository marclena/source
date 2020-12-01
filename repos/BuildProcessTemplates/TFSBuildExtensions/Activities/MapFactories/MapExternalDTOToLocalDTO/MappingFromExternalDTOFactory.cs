using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO
{
    internal class MappingFromExternalDTOFactory
    {
        internal static MappingBase GetFor(EnumExternalDTOToLocalDTO entityName)
        {
            switch (entityName)
            {
                case EnumExternalDTOToLocalDTO.ServerProperties:
                    return new MapExternalServerToPropertiesDictionary();
                case EnumExternalDTOToLocalDTO.ApplicationToServerMatching:
                    return new MapExternalApplicationOnPremiseSettingToStringList();
                case EnumExternalDTOToLocalDTO.PlatformSettings:
                    return new MapBuildPlatformSettingsToLocalDTO();
                default:
                    throw new NotImplementedException(string.Format("Missing mapping for {0} in Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDomainToDTO.", entityName.ToString()));
            }
        }
    }
}
