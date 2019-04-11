using System.Collections.Generic;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories
{
    internal abstract class MappingBase
    {
        internal IEnumerable<TOutput> GetCollection<TInput, TOutput>(IEnumerable<TInput> source)
            where TInput : class
            where TOutput : class
        {
            if (source == null) { yield break; }

            foreach (var item in source)
            {
                yield return Get<TInput, TOutput>(item);
            }
        }

        internal abstract TOutput Get<TInput, TOutput>(TInput source) 
            where TInput : class
            where TOutput : class;
    }
}