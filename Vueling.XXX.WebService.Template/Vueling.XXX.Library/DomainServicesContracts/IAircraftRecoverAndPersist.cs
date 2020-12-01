using System;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Library.DomainServicesContracts
{
    public interface IAircraftRecoverAndPersist
    {

        Aircraft GetAircraftFromRepository(Aircraft aircraft);
        
        bool UpdateAircraft(Aircraft aircraft);
        
        void ValidateAircraftContent(Aircraft aircraft, string flightNumber, DateTime departureDate);

    }
}
