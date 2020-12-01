using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.DomainServicesImplementations
{
    [RegisterServiceAttribute]
    public class FleetRecoverAndPersist : IFleetRecoverAndPersist
    {

        private readonly Vueling.XXX.Library.InfrastructureContracts.IFleetRepository _fleetRepository;

        public FleetRecoverAndPersist(Vueling.XXX.Library.InfrastructureContracts.IFleetRepository fleetRepository)
        {
            _fleetRepository = fleetRepository;
        }

        public bool CreateAircraft(Aircraft aircraft)
        {
            return _fleetRepository.Create(aircraft) >= 0;
        }

        public bool DeleteAircraft(Aircraft aircraft)
        {
            return _fleetRepository.Delete(aircraft) >= 0;
        }

    }
}
