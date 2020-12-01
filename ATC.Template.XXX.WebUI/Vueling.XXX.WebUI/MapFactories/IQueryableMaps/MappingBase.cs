using System.Linq;

namespace Vueling.XXX.WebUI.MapFactories.IQueryableMaps
{
    internal abstract class MappingBase
    {
        internal IQueryable<TOutput> GetCollection<TInput, TOutput>(IQueryable<TInput> source)
            where TInput : class
            where TOutput : class
        {

            return source.Select(x => Get<TInput, TOutput>(x));

            //if (source == null) { yield break; }

            //foreach (var item in source)
            //{
            //    yield return Get<TInput, TOutput>(item);
            //}
        }

        internal abstract TOutput Get<TInput, TOutput>(TInput source) 
            where TInput : class
            where TOutput : class;
    }
}