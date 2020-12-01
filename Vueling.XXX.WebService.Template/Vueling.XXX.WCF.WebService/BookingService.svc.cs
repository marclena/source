using log4net;
using System.Collections.Generic;
using System.Diagnostics;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.WCF.WebService.DTO;
using Vueling.XXX.WCF.WebService.MapFactories.ApplicationDTOToMapWebServiceDTO;

namespace Vueling.XXX.WCF.WebService
{
    [RegisterServiceAttribute]
    public class BookingService : Vueling.XXX.WCF.WebService.IBookingService 
    {
        static ILog Logger = LogManager.GetLogger(typeof(BookingService));
        private readonly Vueling.XXX.Contracts.ServiceLibrary.IBookingApplicationServices _BookingApplicationServices;
        private readonly Vueling.XXX.Contracts.ServiceLibrary.IBookingBusinessApplicationServices _BookingBusinessApplicationServices;

        public BookingService(Vueling.XXX.Contracts.ServiceLibrary.IBookingApplicationServices _IBookingApplicationServices,
            Vueling.XXX.Contracts.ServiceLibrary.IBookingBusinessApplicationServices _IBookingBusinessApplicationServices)
        {
            _BookingApplicationServices = _IBookingApplicationServices;
            _BookingBusinessApplicationServices = _IBookingBusinessApplicationServices;
        }

        public string CreateBooking(int amount)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            int saved = _BookingApplicationServices.CreateBooking(amount);

            watch.Stop();

            return string.Format("Saved {0}. Elapsed {1}", saved, watch.Elapsed);
        }

        public List<BookingResponse> GetBookings(int page, int pageSize)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var bookings = _BookingApplicationServices.GetActivesByPages(page, pageSize);

                var bookingResponses = GetMappedBookings(bookings);

                watch.Stop();

                Logger.Info(string.Format("Retrieved {0} bookings. Elapsed {1}", bookingResponses.Count, watch.Elapsed));

                return bookingResponses;
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public List<BookingResponse> GetCanceledBookings(int page, int pageSize)
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();

                var bookings = _BookingApplicationServices.GetCanceledByPages(page, pageSize);

                var bookingResponses = GetMappedBookings(bookings);

                watch.Stop();

                Logger.Info(string.Format("Retrieved {0} bookings canceled. Elapsed {1}", bookingResponses.Count, watch.Elapsed));

                return bookingResponses;
            }
            catch (System.Exception ex)
            {
                Logger.Error(ex);
                throw;
            }
        }

        public int InfuriateClients()
        {
            return _BookingApplicationServices.ChangeFlights();
        }

        public int MakeManyFriends()
        {
            return _BookingApplicationServices.DividePrices();
        }

        private List<BookingResponse> GetMappedBookings(List<Contracts.ServiceLibrary.DTO.BookingDTO> bookings)
        {
            List<BookingResponse> bookingResponses = new List<BookingResponse>();

            var mapping = MappingToWCFFactory.GetFor(DtoToWCFEnum.Booking);

            foreach (var booking in bookings)
            {
                var mappedBooking = mapping.Get<Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO, BookingResponse>(booking);

                if (mappedBooking != null)
                {
                    SetPriceAndRoute(mappedBooking, booking);
                    bookingResponses.Add(mappedBooking);
                }
            }

            return bookingResponses;
        }

        private void SetPriceAndRoute(BookingResponse bookingResponse, Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO bookingDto)
        {
            bookingResponse.Route = _BookingBusinessApplicationServices.GetRoute(bookingDto);
            bookingResponse.TotalPrice = _BookingBusinessApplicationServices.GetTotalPrice(bookingDto);
        }

    }
}
