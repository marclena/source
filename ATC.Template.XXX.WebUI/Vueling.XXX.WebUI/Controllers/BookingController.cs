using log4net;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Web.Mvc;
using Vueling.XXX.WebUI.Configuration;
using Vueling.XXX.WebUI.MapFactories.ApplicationDTOToDataModel;
using Vueling.XXX.WebUI.Models;
using Vueling.XXX.WebUI.Services;

namespace Vueling.XXX.WebUI.Controllers
{
    public class BookingController : Vueling.Web.UI.Library.Controllers.VuelingController
    {
        static ILog Logger = LogManager.GetLogger(typeof(BookingController));

        private readonly IXXXWebUIConfiguration _XXXWebUIConfiguration;
        private readonly Vueling.XXX.Contracts.ServiceLibrary.IBookingApplicationServices _BookingApplicationServices;
        private readonly IBookingViewModelQueryAdapterService _BookingViewModelQueryAdapterService;

        public BookingController(IXXXWebUIConfiguration _IXXXWebUIConfiguration,
            Vueling.Web.UI.Library.Configuration.IWebUILibraryConfiguration _WebUILibraryConfiguration,
            Vueling.XXX.Contracts.ServiceLibrary.IBookingApplicationServices _IBookingApplicationServices,
            IBookingViewModelQueryAdapterService _IBookingViewModelQueryAdapterService)
            : base(_WebUILibraryConfiguration)
        {
            _XXXWebUIConfiguration = _IXXXWebUIConfiguration;
            _BookingApplicationServices = _IBookingApplicationServices;
            _BookingViewModelQueryAdapterService = _IBookingViewModelQueryAdapterService;
        }


        [GridAction]
        public ActionResult Index(GridCommand command)
        {
            if (command.Page == 0)
            {
                command = new GridCommand
                {
                    Page = 1,
                    PageSize = _XXXWebUIConfiguration.DefaultGridPageSize
                };
            }

            ViewData["total"] = GetDataCount(command);

            IEnumerable<BookingViewModel> data = GetData(command);

            return View(data.ToList());
        }

        public ActionResult CreateBooking()
        {
            int amount = 5;

            var activeBookings = _BookingApplicationServices.CreateBooking(amount);

            return RedirectToAction("Index", "Booking");
        }

        [GridAction(EnableCustomBinding = true)]
        public JsonResult SearchBookings(GridCommand command)
        {
            var bookings = new List<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO>();

            int total = GetDataCount(command);

            IEnumerable<BookingViewModel> data = GetData(command);

            ViewData["total"] = total;

            var model = new GridModel { Data = data, Total = total };

            return new JsonResult
            {
                Data = model
            };
        }

        public ActionResult GetCanceledByPages()
        {
            var activeBookings = _BookingApplicationServices.GetCanceledByPages(1, 10);

            return View();
        }

        public ActionResult ChangeFlights()
        {
            var activeBookings = _BookingApplicationServices.ChangeFlights();

            return RedirectToAction("Index");
        }

        public ActionResult DividePrices()
        {
            var activeBookings = _BookingApplicationServices.DividePrices();

            return RedirectToAction("Index");
        }

        private int GetDataCount(GridCommand command)
        {
            IQueryable<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO> data = _BookingApplicationServices.GetAll();
            return _BookingViewModelQueryAdapterService.ApplyFilters(command, data).Count();
        }

        private IEnumerable<BookingViewModel> GetData(GridCommand command)
        {
            var data = _BookingApplicationServices.GetAll();

            data = _BookingViewModelQueryAdapterService.ApplySorting(command, data);

            data = _BookingViewModelQueryAdapterService.ApplyFilters(command, data);

            data = _BookingViewModelQueryAdapterService.ApplyPaging(command, data);

            var mapper = MappingToViewModelFactory.GetFor(EnumViewModel.BookingViewModel);
            return mapper.GetCollection<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO, BookingViewModel>(data.ToList());
        }

    }
}
