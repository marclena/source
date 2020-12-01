using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.InfrastructureContracts
{
    public interface IAircraftRepository
    {

        Aircraft GetAircraft(Aircraft aircraft);

        int Update(Aircraft aircraft);

        void Rollback(Aircraft aircraft);

    }
}
