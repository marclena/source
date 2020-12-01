using Vueling.Extensions.Library.DI;
using Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain;

namespace Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository
{
    [RegisterServiceAttribute]
    public class AircraftRepository : RepositoryBase<Library.Entities.Aircraft, Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository.Aircraft>, Vueling.XXX.Library.InfrastructureContracts.IAircraftRepository
    {

        public AircraftRepository(IAircraftContext dbContext)
        {
            Context = dbContext.Context;
        }

        public Library.Entities.Aircraft GetAircraft(Library.Entities.Aircraft aircraftEntity)
        {
            var dbAircraft = Find(aircraftEntity);

            MapConceptualModelToDomainFactoryBase factory = SwitcherRepositoryToEntityFactory.GetFactoryFor(typeof(Repositories.AircraftRepository.Aircraft));
            var aircraftEntityReturned = factory.GetEntityFromDbObject<Repositories.AircraftRepository.Aircraft, Vueling.XXX.Library.Entities.Aircraft>(dbAircraft);

            return aircraftEntityReturned;
        }

        public new int Update(Library.Entities.Aircraft aircraftEntity)
        {
            var dbAircraft = base.Update(aircraftEntity);

            return Persist(dbAircraft);
        }

        public void Rollback(Library.Entities.Aircraft aircraftEntity)
        {
            base.Rollback(aircraftEntity);
        }

    }
}