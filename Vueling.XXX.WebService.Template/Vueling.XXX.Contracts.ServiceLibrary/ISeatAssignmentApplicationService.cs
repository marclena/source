using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Contracts.ServiceLibrary
{
    public interface ISeatAssignmentApplicationService
    {

        bool AssignSeatWithValidation(FlightDTO flight, SeatDTO seat);

        bool UnassignSeatWithValidation(FlightDTO flight, SeatDTO seat);

        bool ChangeSeatWithValidation(FlightDTO flight, SeatDTO oldSeat, SeatDTO newSeat);

    }
}
