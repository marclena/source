using System;
using System.Globalization;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.Exceptions;

namespace Vueling.XXX.Library.DomainServicesImplementations
{
    [RegisterServiceAttribute]
    public class AircraftRecoverAndPersist : Vueling.XXX.Library.DomainServicesContracts.IAircraftRecoverAndPersist
    {

        private readonly Vueling.XXX.Library.InfrastructureContracts.IAircraftRepository _aircraftRepository;

        public AircraftRecoverAndPersist(Vueling.XXX.Library.InfrastructureContracts.IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public Aircraft GetAircraftFromRepository(Aircraft aircraftToGet)
        {
            var aircraftFromDb = _aircraftRepository.GetAircraft(aircraftToGet);

            ValidateAircraftContent(aircraftFromDb, aircraftToGet.FlightNumber, aircraftToGet.DepartureDate);

            return aircraftFromDb;
        }

        public bool UpdateAircraft(Aircraft aircraft)
        {
            return _aircraftRepository.Update(aircraft) >= 0;
        }

        public void ValidateAircraftContent(Aircraft aircraft, string flightNumber, DateTime departureDate)
        {
            //call specification pattern: seats.count > 0.......
            if (aircraft == null)
            {
                throw new AircraftParamIsNullException(string.Format(CultureInfo.InvariantCulture, "Aircraft with flight number {0} and departure date {1} not found.", flightNumber, departureDate));
            }
        }

    }
}
