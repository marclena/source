using System;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDTOToDomain;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Impl.ServiceLibrary
{
    [RegisterService]
    public class SeatAssignmentApplicationService : ISeatAssignmentApplicationService
    {
        private ISeatAssignment _seatAssignment;

        public SeatAssignmentApplicationService(ISeatAssignment seatAssignment)
        {
            _seatAssignment = seatAssignment;
        }

        public bool AssignSeatWithValidation(FlightDTO flight, SeatDTO seatDTO)
        {
            var result = false;

            ValidateDTOsProperties(flight, seatDTO);

            var aircraft = new Aircraft(flight.Identifier, flight.DepartureTime);

            if (_seatAssignment.ValidateTimeLimitBeforeFlightForAssignment(aircraft))
            {

                var factory = MappingToDomainFactory.GetFor(EnumDomain.Seat);
                Seat seat = factory.Get<SeatDTO, Seat>(seatDTO);

                result = _seatAssignment.Assign(aircraft, seat);

            }
            
            return result;
        }

        private void ValidateDTOsProperties(FlightDTO flight, SeatDTO seatDTO)
        {
            if (flight.Identifier != null && flight.Identifier.Length == 0) throw new ArgumentException("Empty flightNumber");
            if (flight.DepartureTime == DateTime.MinValue) throw new ArgumentException("departureDate not set");
            if (seatDTO == default(SeatDTO)) throw new ArgumentNullException("Null seatDTO");
        }


        public bool UnassignSeatWithValidation(FlightDTO flight, SeatDTO seatDTO)
        {
            var result = false;

            ValidateDTOsProperties(flight, seatDTO);

            var aircraft = new Aircraft(flight.Identifier, flight.DepartureTime);

            if (_seatAssignment.ValidateTimeLimitBeforeFlightForAssignment(aircraft))
            {

                var factory = MappingToDomainFactory.GetFor(EnumDomain.Seat);
                var seat = factory.Get<SeatDTO, Seat>(seatDTO);

                result = _seatAssignment.Unassign(aircraft, seat);

            }

            return result;
        }

        public bool ChangeSeatWithValidation(FlightDTO flight, SeatDTO oldSeatDTO, SeatDTO newSeatDTO)
        {
            var result = this.UnassignSeatWithValidation(flight, oldSeatDTO) && this.AssignSeatWithValidation(flight, newSeatDTO);
            return result;
        }
    }
}
