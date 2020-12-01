using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.ToDTOAsIQueryable
{
    internal abstract class IQueryableMappingBase
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