using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.InfrastructureContracts
{
    public interface IFleetRepository
    {

        int Create(Aircraft aircraft);

        int Delete(Aircraft aircraft);

    }
}
