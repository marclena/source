using Vueling.Extensions.Library.DI;

namespace Vueling.XXX.DB.Infrastructure.Repositories.FleetRepository
{
    [RegisterServiceAttribute]
    public class FleetRepository : RepositoryBase<Library.Entities.Aircraft, Vueling.XXX.DB.Infrastructure.Repositories.FleetRepository.Aircraft>, Vueling.XXX.Library.InfrastructureContracts.IFleetRepository
    {

        public FleetRepository(IFleetContext dbContext)
        {
            Context = dbContext.Context;
        }

        public new int Create(Library.Entities.Aircraft aircraftEntity)
        {
            var dbNewAircraft = base.Create(aircraftEntity);

            return Persist(dbNewAircraft);
        }

        public new int Delete(Library.Entities.Aircraft aircraftEntity)
        {
            var dbDeletedAircraft = base.Delete(aircraftEntity);

            return Persist(dbDeletedAircraft);
        }

    }
}
