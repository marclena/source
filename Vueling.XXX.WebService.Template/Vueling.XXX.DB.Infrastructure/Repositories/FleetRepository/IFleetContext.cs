using System.Data.Objects;

namespace Vueling.XXX.DB.Infrastructure.Repositories.FleetRepository
{
    public interface IFleetContext
    {
        ObjectContext Context{ get; }
    }
}
