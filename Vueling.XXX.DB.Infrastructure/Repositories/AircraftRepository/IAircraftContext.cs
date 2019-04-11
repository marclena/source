using System.Data.Objects;

namespace Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository
{
    public interface IAircraftContext
    {
        ObjectContext Context{ get; }
    }
}
