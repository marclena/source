using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.WCF.WebService.DTO;
using Vueling.XXX.WCF.WebService.MapFactories.ApplicationDTOToMapWebServiceDTO;
using Vueling.XXX.WCF.WebService.MapFactories.MapWebServiceDTOToApplicationDTO;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.DB.Infrastructure.Exceptions;
using Vueling.XXX.Library.Exceptions;
using Vueling.XXX.WCF.WebService.Helpers;
using System.Diagnostics;


namespace Vueling.XXX.WCF.WebService
{
    [RegisterServiceAttribute]
    public class SeatReservationForAircraftsWebService : ISeatReservationForAircraftsWebService
    {

        private const string SUCCESS_MESSAGE_TO_CLIENT = "Seat reserved.";
        private const string APPLICATION_ERROR_MESSAGE_TO_CLIENT = "Error reservating the seat due an application error. Seat not reserved.";
        private const string BUSINESS_ERROR_MESSAGE_TO_CLIENT = "Error reservating the seat due a business error (flight not found, incorrect internal params, not access to database). Seat not reserved.";


        #region .: constructors :.

        public SeatReservationForAircraftsWebService(ISeatAssignmentApplicationService seatAssigmentService)
        {
            _seatAssigmentService = seatAssigmentService;
        }

        #endregion

        #region .: properties :.

        public ISeatAssignmentApplicationService _seatAssigmentService { get; set; }

        #endregion

        #region .: public methods :.

        public string ReserveASeat(string flighIdentifier, DateTime departureTime, int row, string colum)
        {

            string returnedMessage = string.Empty;
            List<SeatDTO> seats = new List<SeatDTO>();

            ValidateParametersForReserveSeat(flighIdentifier, departureTime, row, colum);

            ReservationSeatRequestDTO reservationSeatRequestDTO = new ReservationSeatRequestDTO(flighIdentifier, departureTime, row, colum);

            var reservationSeatRequestDTOToFlightDTOFactory = MappingFromWCFFactory.GetFactoryFor(EnumApplicationDTO.FlightDTO);
            FlightDTO flight = reservationSeatRequestDTOToFlightDTOFactory.Get<ReservationSeatRequestDTO, FlightDTO>(reservationSeatRequestDTO);

            var reservationSeatRequestDTOToSeatDTOFactory = MappingFromWCFFactory.GetFactoryFor(EnumApplicationDTO.SeatDTO);
            SeatDTO seat = reservationSeatRequestDTOToSeatDTOFactory.Get<ReservationSeatRequestDTO, SeatDTO>(reservationSeatRequestDTO);
            seats.Add(seat);

            returnedMessage = CallApplicationMethods(SeatReservationForAircraftsOperationsEnum.ReserveASeat, flight, seats);

            return returnedMessage;

        }

        public string ChangeASeatReservation(string flighIdentifier, DateTime departureTime, int currentRow, string currentColum, int newRow, string newColum)
        {

            string returnedMessage = string.Empty;
            List<SeatDTO> seats = new List<SeatDTO>();

            ValidateParametersForReserveSeat(flighIdentifier, departureTime, currentRow, currentColum);
            ValidateParametersForReserveSeat(flighIdentifier, departureTime, newRow, newColum);

            ReservationSeatRequestDTO reservationSeatRequestDTOForCurrentSeat = new ReservationSeatRequestDTO(flighIdentifier, departureTime, currentRow, currentColum);
            ReservationSeatRequestDTO reservationSeatRequestDTOForNewSeat = new ReservationSeatRequestDTO(flighIdentifier, departureTime, newRow, newColum);

            var reservationSeatRequestDTOToFlightDTOFactory = MappingFromWCFFactory.GetFactoryFor(EnumApplicationDTO.FlightDTO);
            FlightDTO flight = reservationSeatRequestDTOToFlightDTOFactory.Get<ReservationSeatRequestDTO, FlightDTO>(reservationSeatRequestDTOForCurrentSeat);

            var reservationSeatRequestDTOToSeatDTOFactory = MappingFromWCFFactory.GetFactoryFor(EnumApplicationDTO.SeatDTO);
            SeatDTO currentSeat = reservationSeatRequestDTOToSeatDTOFactory.Get<ReservationSeatRequestDTO, SeatDTO>(reservationSeatRequestDTOForCurrentSeat);
            seats.Add(currentSeat);
            SeatDTO newSeat = reservationSeatRequestDTOToSeatDTOFactory.Get<ReservationSeatRequestDTO, SeatDTO>(reservationSeatRequestDTOForNewSeat);
            seats.Add(newSeat);

            returnedMessage = CallApplicationMethods(SeatReservationForAircraftsOperationsEnum.ChangeASeatReservation, flight, seats);

            return returnedMessage;

        }

        #endregion

        private void ValidateParametersForReserveSeat(string flighIdentifier, DateTime departureTime, int row, string colum)
        {
            if (flighIdentifier != null && flighIdentifier.Length == 0) throw new ArgumentException("FlightNumber is empty.");
            if (departureTime == DateTime.MinValue) throw new ArgumentException("departureDate not set");
            if (row < 0) throw new ArgumentException("Row number is not allow.");
            if (colum == null) throw new ArgumentNullException("Colum is null.");
        }

        private string CallApplicationMethods(SeatReservationForAircraftsOperationsEnum clientRequestedMethod, FlightDTO flight, List<SeatDTO> seats)
        {

            string returned = SUCCESS_MESSAGE_TO_CLIENT;

            try
            {

                if (clientRequestedMethod == SeatReservationForAircraftsOperationsEnum.ReserveASeat)
                {

                    if (!_seatAssigmentService.AssignSeatWithValidation(flight, seats.First())) returned = BUSINESS_ERROR_MESSAGE_TO_CLIENT;

                }
                else if (clientRequestedMethod == SeatReservationForAircraftsOperationsEnum.ChangeASeatReservation)
                {
                    if (!_seatAssigmentService.ChangeSeatWithValidation(flight, seats.First(), seats.Last())) returned = BUSINESS_ERROR_MESSAGE_TO_CLIENT;
                }
            }
            catch (AircraftNotFoundOnDatabaseException infrastructureException)
            {

                returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
                Trace.TraceError("Flight not found :" + infrastructureException.Message);

            }
            catch (AircraftAlreadyExistingOnDatabaseException infrastructureException)
            {

                returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
                Trace.TraceError("Flight already existing :" + infrastructureException.Message);

            }
            catch (ArgumentNullException Exception)
            {

                returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
                Trace.TraceError("Wrong parameter passed to a method: " + Exception.Message);

            }
            catch (ArgumentException Exception)
            {

                returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
                Trace.TraceError("Wrong parameter passed to a method: " + Exception.Message);

            }
            catch (UpdateToRepositoryException domainException)
            {

                returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
                Trace.TraceError("Error updating database: " + domainException.Message);

            }

            return returned;

        }
    }
}
