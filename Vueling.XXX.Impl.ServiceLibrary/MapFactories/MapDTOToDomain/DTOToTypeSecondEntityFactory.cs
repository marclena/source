using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Library;

namespace Vueling.XXX.Impl.ServiceLibrary.MapDTOToDomain
{

    public class DTOToTypeSecondEntityFactory : IDTOToEntityFactory
    {

        public IEnumerable<TEntity> GetEntitiesFromDTOs<TEntity, TDTO>(IEnumerable<TDTO> enumerableDTOs)
        {

            throw new NotImplementedException("GetEntitiesFromDTOs is not implemented for DTOToTypeSecondEntityFactory in ServiceLibrary.");

        }

        public TEntity GetEntityFromDTO<TEntity, TDTO>(TDTO dto)
        {

            var exampleDataTransferObject = dto as ExampleDataTransferObject;

            TypeSecondEntity second = new TypeSecondEntity();
            second.Quantity.Number = exampleDataTransferObject.paramList.ElementAt(1);

            return (TEntity)(object)second;

        }
    }


}
