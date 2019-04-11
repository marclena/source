using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.XXX.MessageConsumer.WindowsService.MapFactories.MapMessageToApplicationDTO
{
    internal abstract class MapMessageToApplicationDTOFactoryBase
    {

        internal virtual IEnumerable<TOutput> MapToDTOs<TInput, TOutput>(IEnumerable<TInput> dtos)
            where TInput : class
            where TOutput : class
        {
            if (dtos == null)
            {
                yield break;
            }

            foreach (var item in dtos)
            {
                yield return MapToDTO<TInput, TOutput>(item);
            }
        }

        internal abstract TOutput MapToDTO<TInput, TOutput>(TInput dto)
            where TInput : class
            where TOutput : class;


    }
}
