using System.Collections.Generic;

namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel
{
    internal abstract class MapDomainToConceptualModelFactoryBase
    {

        #region DbObjects from Entities.


        internal virtual IEnumerable<TOutput> GetDbObjectsFromEntities<TInput, TOutput>(IEnumerable<TInput> entities)
            where TInput : class
            where TOutput : class
        {
            if (entities == null) { yield break; }

            foreach (var item in entities)
            {
                yield return GetDbObjectFromEntity<TInput, TOutput>(item);
            }
        }

        internal abstract TOutput GetDbObjectFromEntity<TInput, TOutput>(TInput entity)
            where TInput : class
            where TOutput : class;

        #endregion

    }
}
