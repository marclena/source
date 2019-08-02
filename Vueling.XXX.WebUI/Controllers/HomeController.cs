using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vueling.WebUI.Helpers;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

using Vueling.XXX.Library.Exceptions;
using Vueling.XXX.WebUI.MapFactories.DataModelToApplicationDTO;
using Vueling.XXX.WebUI.Models;
using System.Globalization;

namespace Vueling.XXX.WebUI.Controllers
{
    public class HomeController : Vueling.Web.UI.Library.Controllers.VuelingController
    {
        private const string SUCCESS_MESSAGE_TO_CLIENT = "Seat reserved.";
        private const string APPLICATION_ERROR_MESSAGE_TO_CLIENT = "Error reservating the seat due an application error. Seat not reserved.";
        private const string BUSINESS_ERROR_MESSAGE_TO_CLIENT = "Error reservating the seat due a business error (flight not found, incorrect internal params, not access to database). Seat not reserved.";

        #region .: contructors :.

        public HomeController(
            Vueling.Web.UI.Library.Configuration.IWebUILibraryConfiguration _WebUILibraryConfiguration,
            ISeatAssignmentApplicationService seatAssigmentService)
            : base(_WebUILibraryConfiguration)
        {
            _seatAssigmentService = seatAssigmentService;
            _httpcontext = new HttpContextWrapper(System.Web.HttpContext.Current);
        }

        public HomeController(Vueling.Web.UI.Library.Configuration.IWebUILibraryConfiguration _WebUILibraryConfiguration,
            ISeatAssignmentApplicationService seatAssigmentService, HttpContextBase httpcontext)
            : base(_WebUILibraryConfiguration)
        {
            _seatAssigmentService = seatAssigmentService;
            _httpcontext = httpcontext;
        }

        #endregion

        #region .: properties :.

        public ISeatAssignmentApplicationService _seatAssigmentService { get; set; }
        private HttpContextBase _httpcontext { get; set; }

        #endregion


        #region .: public methods :.

        public ActionResult Index()
        {
            IndexViewModel viewmodel;
            string returnedMessage = string.Empty;
            List<SeatDTO> seats = new List<SeatDTO>();

            if (_httpcontext.Request.HttpMethod == "POST")
            {

                DateTime departureTime = ConvertDepartureTimeToDateTime(_httpcontext.Request.Form["departureTimeString"]);
                ValidateParametersForReserveSeat(_httpcontext.Request.Form["flighIdentifier"], departureTime, Convert.ToInt16(_httpcontext.Request.Form["row"], CultureInfo.InvariantCulture), _httpcontext.Request.Form["column"]);

                ReservationSeatDataModel reservationSeatRequestDTO = new ReservationSeatDataModel(_httpcontext.Request.Form["flighIdentifier"], departureTime, Convert.ToInt16(_httpcontext.Request.Form["row"], CultureInfo.InvariantCulture), _httpcontext.Request.Form["column"]);

                MapDataModelToApplicationDTOFactoryBase reservationSeatRequestDTOToFlightDTOFactory = SwictherDataModelToApplicationDTO.GetFactoryFor(EnumApplicationDTO.FlightDTO);
                FlightDTO flight = reservationSeatRequestDTOToFlightDTOFactory.GetApplicationDTOFromWebServiceDTO<ReservationSeatDataModel, FlightDTO>(reservationSeatRequestDTO);

                MapDataModelToApplicationDTOFactoryBase reservationSeatRequestDTOToSeatDTOFactory = SwictherDataModelToApplicationDTO.GetFactoryFor(EnumApplicationDTO.SeatDTO);
                SeatDTO seat = reservationSeatRequestDTOToSeatDTOFactory.GetApplicationDTOFromWebServiceDTO<ReservationSeatDataModel, SeatDTO>(reservationSeatRequestDTO);
                seats.Add(seat);

                returnedMessage = CallApplicationMethods(SeatReservationForAircraftsOperationsEnum.ReserveASeat, flight, seats);

            }
            else returnedMessage = "Submit any integer values";

            viewmodel = new IndexViewModel { Message = returnedMessage, LabelId = Guid.NewGuid().ToString() };

            return View(viewmodel);
        }

        #endregion

        #region . : private methods :.

        private static DateTime ConvertDepartureTimeToDateTime(string departureTimeString)
        {
            //201303230000
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
            FlighIdentifierNotNullValidate(flighIdentifier);
            DepartureTimeNotNullValidate(departureTime);
            RowValidate(row);
            ColumValidate(colum);
        }

        private void ColumValidate(string colum)
        {
            if (colum == null)
                ThrowArgumentException("Colum is null.");
        }

        private void RowValidate(int row)
        {
            if (row < 0)
                ThrowArgumentException("Row number is not allow.");
        }

        private void DepartureTimeNotNullValidate(DateTime departureTime)
        {
            if (departureTime == DateTime.MinValue)
                ThrowArgumentException("DepartureDate is not set.");
        }

        private void FlighIdentifierNotNullValidate(string flighIdentifier)
        {
            if (flighIdentifier != null && flighIdentifier.Length == 0)
                ThrowArgumentException("FlightNumber is empty.");
        }

        private void ThrowArgumentException(string message)
        {
            throw new ArgumentException(message);
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
                else if (clientRequestedMethod == SeatReservationForAircraftsOperationsEnum.ChangeASeatReservation && !_seatAssigmentService.ChangeSeatWithValidation(flight, seats.First(), seats.Last()))
                {
                    returned = BUSINESS_ERROR_MESSAGE_TO_CLIENT;
                }
            }
            //catch (AircraftNotFoundOnDatabaseException infrastructureException)
            //{

            //    returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
            //    Trace.TraceError("Flight not found :" + infrastructureException.Message);

            //}
            //catch (AircraftAlreadyExistingOnDatabaseException infrastructureException)
            //{

            //    returned = APPLICATION_ERROR_MESSAGE_TO_CLIENT;
            //    Trace.TraceError("Flight already existing :" + infrastructureException.Message);

            //}
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
