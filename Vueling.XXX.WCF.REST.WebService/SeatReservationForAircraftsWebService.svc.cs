using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Activation;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.DB.Infrastructure.Exceptions;
using Vueling.XXX.Library.Exceptions;
using Vueling.XXX.WCF.REST.WebService.DTO;
using Vueling.XXX.WCF.REST.WebService.Helpers;
using Vueling.XXX.WCF.REST.WebService.MapFactories.MapWebServiceDTOToApplicationDTO;

[assembly: CLSCompliant(true)]
namespace Vueling.XXX.WCF.REST.WebService
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
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

        public string ReserveASeat(string flighIdentifier, string departureTimeString, string row, string colum)
        {
            string returnedMessage = string.Empty;
            List<SeatDTO> seats = new List<SeatDTO>();

            DateTime departureTime = ConvertDepartureTimeToDateTime(departureTimeString);
            ValidateParametersForReserveSeat(flighIdentifier, departureTime, Convert.ToInt16(row, CultureInfo.InvariantCulture), colum);

            ReservationSeatRequestDTO reservationSeatRequestDTO = new ReservationSeatRequestDTO(flighIdentifier, departureTime, Convert.ToInt16(row, CultureInfo.InvariantCulture), colum);

            MapWebServiceDTOToApplicationDTOFactoryBase reservationSeatRequestDTOToFlightDTOFactory = SwictherWebServiceDTOToApplicationDTO.GetFactoryFor(EnumApplicationDTO.FlightDTO);
            FlightDTO flight = reservationSeatRequestDTOToFlightDTOFactory.GetApplicationDTOFromWebServiceDTO<ReservationSeatRequestDTO, FlightDTO>(reservationSeatRequestDTO);

            MapWebServiceDTOToApplicationDTOFactoryBase reservationSeatRequestDTOToSeatDTOFactory = SwictherWebServiceDTOToApplicationDTO.GetFactoryFor(EnumApplicationDTO.SeatDTO);
            SeatDTO seat = reservationSeatRequestDTOToSeatDTOFactory.GetApplicationDTOFromWebServiceDTO<ReservationSeatRequestDTO, SeatDTO>(reservationSeatRequestDTO);
            seats.Add(seat);

            returnedMessage = CallApplicationMethods(SeatReservationForAircraftsOperationsEnum.ReserveASeat, flight, seats);

            return returnedMessage;
        }

        #endregion

        #region .: private methods :.

        private static DateTime ConvertDepartureTimeToDateTime(string departureTimeString)
        {
            //130320131800, yyyyMMddhhmm
            DateTime departureTime = new DateTime(Convert.ToInt16(departureTimeString.Substring(0, 4), CultureInfo.InvariantCulture),
                Convert.ToInt16(departureTimeString.Substring(4, 2), CultureInfo.InvariantCulture),
                Convert.ToInt16(departureTimeString.Substring(6, 2), CultureInfo.InvariantCulture),
                Convert.ToInt16(departureTimeString.Substring(8, 2), CultureInfo.InvariantCulture),
                Convert.ToInt16(departureTimeString.Substring(10, 2), CultureInfo.InvariantCulture),
                0);
            return departureTime;
        }

        private void ValidateParametersForReserveSeat(string flighIdentifier, DateTime departureTime, int row, string colum)
        {
            if (flighIdentifier != null && flighIdentifier.Length == 0) throw new ArgumentException("FlightNumber is empty.");
            if (departureTime == null) throw new ArgumentNullException("DepartureDate is null.");
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

        #endregion
    }
}
