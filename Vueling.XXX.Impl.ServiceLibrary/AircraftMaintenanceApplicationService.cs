using System.Collections.Generic;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Impl.ServiceLibrary.Configuration;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Impl.ServiceLibrary
{
    [RegisterServiceAttribute]
    public class AircraftMaintenanceApplicationService : IAircraftMaintenanceApplicationService
    {

        private IFleetRecoverAndPersist _fleetRecoverAndPersist;
        private IXXXServiceLibraryConfiguration _xXXServiceLibraryConfiguration;

        public AircraftMaintenanceApplicationService(IXXXServiceLibraryConfiguration serviceLibraryConfiguration, IFleetRecoverAndPersist fleetRecoverAndPersist)
        {
            _fleetRecoverAndPersist = fleetRecoverAndPersist;
            _xXXServiceLibraryConfiguration = serviceLibraryConfiguration;
        }

        public bool CreateNewEmptyAircraft(Contracts.ServiceLibrary.DTO.FlightDTO flight)
        {

            bool returned = false;

            List<Seat> seatsFornewAircraft = new List<Seat>();
            seatsFornewAircraft = _xXXServiceLibraryConfiguration.FlightSeatsConfiguration;

            var aircraft = new Aircraft(flight.Identifier, flight.DepartureTime, seatsFornewAircraft);

            returned = _fleetRecoverAndPersist.CreateAircraft(aircraft);

            return returned;

        }


        public bool ReleaseAircraft(Contracts.ServiceLibrary.DTO.FlightDTO flight)
        {

            bool returned = false;

            var aircraft = new Aircraft(flight.Identifier, flight.DepartureTime);

            returned = _fleetRecoverAndPersist.DeleteAircraft(aircraft);

            return returned;

        }
    }
}
