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
        }

        internal abstract TOutput Get<TInput, TOutput>(TInput source) 
            where TInput : class
            where TOutput : class;
    }
}