using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Library;

namespace Vueling.XXX.Impl.ServiceLibrary.MapDTOToDomain
{
    public class DTOToPositionEntityFactory : IDTOToEntityFactory
    {

        public IEnumerable<TEntity> GetEntitiesFromDTOs<TEntity, TDTO>(IEnumerable<TDTO> enumerableDTOs)
        {

            throw new NotImplementedException("GetEntitiesFromDTOs is not implemented for DTOToModelEntityFactory in ServiceLibrary.");

        }


        public TEntity GetEntityFromDTO<TEntity, TDTO>(TDTO dto)
        {

            var exampleDataTransferObject = dto as ExampleDataTransferObject;

            PositionEntity modelEntity = new PositionEntity();

            var firstEntityMapperFactory = DTOToDomainMapperFactory.GetMapperFactoryFor(EnumDomainEntities.TypeFirstEntity);
            TypeFirstEntity firstEntity = firstEntityMapperFactory.GetEntityFromDTO<TypeFirstEntity, ExampleDataTransferObject>(exampleDataTransferObject);

            var secondEntityMapperFactory = DTOToDomainMapperFactory.GetMapperFactoryFor(EnumDomainEntities.TypeSecondEntity);
            TypeSecondEntity secondEntity = secondEntityMapperFactory.GetEntityFromDTO<TypeSecondEntity, ExampleDataTransferObject>(exampleDataTransferObject);

            var thirdEntityMapperFactory = DTOToDomainMapperFactory.GetMapperFactoryFor(EnumDomainEntities.TypeThirdEntity);
            TypeThirdEntity thirdEntity = thirdEntityMapperFactory.GetEntityFromDTO<TypeThirdEntity, ExampleDataTransferObject>(exampleDataTransferObject);

            modelEntity.first = firstEntity;
            modelEntity.second = secondEntity;
            modelEntity.third = thirdEntity;

            return (TEntity)(object)modelEntity;

        }
    }
}
