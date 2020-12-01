using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSBuildExtensions.MapFactories.MapExternalDTOToLocalDTO
{
    internal class MapBuildPlatformSettingsToLocalDTO : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var platformSettings = source as Vueling.Build.Contracts.ServiceLibrary.DTO.PlatformSettings;

            if (platformSettings == null) { throw new InvalidCastException(typeof(TInput).Name); }

            TFSBuildExtensions.DTO.PlatformSettings localPlatformSettings = new TFSBuildExtensions.DTO.PlatformSettings();

            localPlatformSettings.Site = new DTO.Site();

            localPlatformSettings.Site.PhysicalPath = platformSettings.Site.PhysicalPath;

            //@todo add all properties

            return localPlatformSettings as TOutput;
        }
    }
}
