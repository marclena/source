using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.DomainServicesContracts
{
    public interface IFleetRecoverAndPersist
    {

        bool CreateAircraft(Aircraft aircraft);

        bool DeleteAircraft(Aircraft aircraft);

    }
}
