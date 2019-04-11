using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vueling.XXX.Impl.ServiceLibrary.MapDTOToDomain
{
    public class DTOToDomainMapperFactory
    {

        public static IDTOToEntityFactory GetMapperFactoryFor(EnumDomainEntities enumEntities)
        {

            switch (enumEntities)
            {

                case EnumDomainEntities.PositionEntity:
                    return new DTOToPositionEntityFactory();
                case EnumDomainEntities.TypeFirstEntity:
                    return new DTOToTypeFirstEntityFactory();
                case EnumDomainEntities.TypeSecondEntity:
                    return new DTOToTypeSecondEntityFactory();
                case EnumDomainEntities.TypeThirdEntity:
                    return new DTOToTypeThirdEntityFactory();
                default: throw new NotImplementedException(); 
            
            }
        
        }

    }
}
