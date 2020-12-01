using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.DomainServicesContracts
{
    public interface ISeatAssignment
    {
        bool Assign(Aircraft aircraft, Seat seat);

        bool Unassign(Aircraft aircraft, Seat seat);

        bool ValidateTimeLimitBeforeFlightForAssignment(Aircraft aircraft);
    }
}
