using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.XXX.Impl.ServiceLibrary.MapDTOToDomain
{
    public interface IDTOToEntityFactory
    {

        IEnumerable<TEntity> GetEntitiesFromDTOs<TEntity, TDTO>(IEnumerable<TDTO> enumerableDTOs);

        TEntity GetEntityFromDTO<TEntity, TDTO>(TDTO dto);

    }
}
