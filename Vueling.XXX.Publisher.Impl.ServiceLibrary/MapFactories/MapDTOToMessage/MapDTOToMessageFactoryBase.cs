using System.Collections.Generic;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary.MapFactories.MapDTOToMessage
{
    internal abstract class MapDTOToMessageFactoryBase
    {

        #region Entities from DTO.

        internal virtual IEnumerable<TOutput> GetMessageFromApplicationDTOs<TInput, TOutput>(IEnumerable<TInput> dtos)
            where TInput : class
            where TOutput : class
        {
            if (dtos == null)
            {
                yield break;
            }

            foreach (var item in dtos)
            {
                yield return GetMessageFromApplicationDTO<TInput, TOutput>(item);
            }
        }

        internal abstract TOutput GetMessageFromApplicationDTO<TInput, TOutput>(TInput dto)
            where TInput : class
            where TOutput : class;

        #endregion

    }
}
