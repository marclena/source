using System.Collections.Generic;

namespace Vueling.XXX.WebUI.MapFactories.DataModelToApplicationDTO
{
    internal abstract class MapDataModelToApplicationDTOFactoryBase
    {

        #region Entities from DTO.

        internal virtual IEnumerable<TOutput> GetApplicationDTOsFromWebServiceDTOs<TInput, TOutput>(IEnumerable<TInput> dtos)
            where TInput : class
            where TOutput : class
        {
            if (dtos == null)
            {
                yield break;
            }

            foreach (var item in dtos)
            {
                yield return GetApplicationDTOFromWebServiceDTO<TInput, TOutput>(item);
            }
        }

        internal abstract TOutput GetApplicationDTOFromWebServiceDTO<TInput, TOutput>(TInput dto)
            where TInput : class
            where TOutput : class;

        #endregion

    }
}