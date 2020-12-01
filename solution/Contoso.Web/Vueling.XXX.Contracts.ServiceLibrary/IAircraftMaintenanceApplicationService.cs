using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Contracts.ServiceLibrary
{
    public interface IAircraftMaintenanceApplicationService
    {

        bool CreateNewEmptyAircraft(FlightDTO flight);

        bool ReleaseAircraft(FlightDTO flight);

    }
}
