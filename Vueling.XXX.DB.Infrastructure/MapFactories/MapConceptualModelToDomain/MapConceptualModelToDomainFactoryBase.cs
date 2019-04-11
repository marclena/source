using System.Collections.Generic;

namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain
{
    internal abstract class MapConceptualModelToDomainFactoryBase
    {

        #region Entities from DbObjects.

        internal virtual IEnumerable<TOutput> GetEntitiesFromDbObjects<TInput, TOutput>(IEnumerable<TInput> dbobjects)
            where TInput : class
            where TOutput : class
        {
            if (dbobjects == null)
            {
                yield break;
            }

            foreach (var item in dbobjects)
            {
                yield return GetEntityFromDbObject<TInput, TOutput>(item);
            }
        }

        internal abstract TOutput GetEntityFromDbObject<TInput, TOutput>(TInput dbobject)
            where TInput : class
            where TOutput : class;

        #endregion

    }
}
