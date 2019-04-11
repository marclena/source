using System;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.Exceptions;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.Configuration;

namespace Vueling.XXX.Library.DomainServicesImplementations
{
    [RegisterServiceAttribute]
    public class SeatAssignment : Vueling.XXX.Library.DomainServicesContracts.ISeatAssignment
    {

        private readonly IAircraftRecoverAndPersist _aircraftRecoverAndPersist;
        private readonly IXXXLibraryConfiguration _xXXLibraryConfiguration;


        public SeatAssignment(IAircraftRecoverAndPersist aircraftRecoverAndPersist, IXXXLibraryConfiguration xXXLibraryConfiguration)
        {
            _aircraftRecoverAndPersist = aircraftRecoverAndPersist;
            _xXXLibraryConfiguration = xXXLibraryConfiguration;
        }


        public bool Assign(Aircraft aircraft, Seat seat)
        {
            var aircraftFromDb = _aircraftRecoverAndPersist.GetAircraftFromRepository(aircraft);
            Seat seatToAssign = new Seat 
            { 
                Availability = AvailabilityEnum.Available, 
                Column = seat.Column, 
                Row = seat.Row
            };

            bool isAssignmentOK = aircraftFromDb.Assign(seatToAssign);
            if (!isAssignmentOK) 
            { 
                return false; 
            }

            bool isUpdateOK = _aircraftRecoverAndPersist.UpdateAircraft(aircraftFromDb);
            if (!isUpdateOK) 
            {
                /*implement some rollback logic */
                throw new UpdateToRepositoryException();
            }
            
            return true;
        }

        public bool Unassign(Aircraft aircraft, Seat seat)
        {
            if (seat == null || (seat.Row.ToString() != null && seat.Row.ToString().Length == 0)
                || (seat.Column.ToString() != null && seat.Column.ToString().Length == 0)) 
            {
                throw new RowOrColumnParamsAreNullOrEmptyException("Parameters row and column are required.");
            }

            Seat seatToAssign = new Seat 
            { 
                Availability = AvailabilityEnum.Busy, 
                Column = seat.Column, 
                Row = seat.Row 
            };

            bool isUnassignOK = aircraft.Unassign(seatToAssign);
            if (!isUnassignOK) 
            { 
                return false; 
            }

            bool isUpdateOK = _aircraftRecoverAndPersist.UpdateAircraft(aircraft);
            if (!isUpdateOK)
            {
                /*implement some rollback logic */
                return false;
            }

            return true;
        }

        public bool ValidateTimeLimitBeforeFlightForAssignment(Aircraft aircraft)
        {

            var result = aircraft.DepartureDate.Subtract(DateTime.Now).Hours >= _xXXLibraryConfiguration.TimeSalesCloseBeforeFlight;
            return result;
        
        }
        
    }
}
