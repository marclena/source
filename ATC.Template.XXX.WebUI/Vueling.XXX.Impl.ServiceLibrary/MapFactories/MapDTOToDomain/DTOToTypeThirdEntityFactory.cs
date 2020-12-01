using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Library;

namespace Vueling.XXX.Impl.ServiceLibrary.MapDTOToDomain
{
    public class DTOToTypeThirdEntityFactory : IDTOToEntityFactory
    {

        public IEnumerable<TEntity> GetEntitiesFromDTOs<TEntity, TDTO>(IEnumerable<TDTO> enumerableDTOs)
        {

            throw new NotImplementedException("GetEntitiesFromDTOs is not implemented for DTOToTypeSecondEntityFactory in ServiceLibrary.");

        }

        public TEntity GetEntityFromDTO<TEntity, TDTO>(TDTO dto)
        {

            var exampleDataTransferObject = dto as ExampleDataTransferObject;

            TypeThirdEntity third = new TypeThirdEntity();
            third.Quantity.Number = exampleDataTransferObject.paramList.ElementAt(2);

            return (TEntity)(object)third;

        }
    }
}
