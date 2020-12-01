using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.InfrastructureContracts;
using Vueling.XXX.Library.BookingServices;

namespace Vueling.XXX.Library.DomainServicesImplementations
{
    [RegisterServiceAttribute]
    public class BookingDomainServices : Vueling.XXX.Library.DomainServicesContracts.IBookingDomainServices
    {
        /*
         * Scene using context with lazyloading disabled and work with aggregate.
         */
        const bool trackingStateForReading = false;
        const bool trackingStateForEdit = true;

        private readonly IUnitOfWorkBooking _UnitOfWorkBooking;
        private readonly Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Booking> _BookingRepository;
        private readonly IBookingBulkModifierServices _BookingBulkModifierServices;

        public BookingDomainServices(IUnitOfWorkBooking _IUnitOfWorkBooking, 
            IBookingBulkModifierServices _IBookingBulkModifierServices)
        {
            _UnitOfWorkBooking = _IUnitOfWorkBooking;
            _BookingRepository = _UnitOfWorkBooking.GetRepository<Booking>();

            _BookingBulkModifierServices = _IBookingBulkModifierServices;
        }

        public int CreateSampleBooking(int amount)
        {
            var newBookings = InMemoryBookingProvider.CreateBatch(amount);

            _BookingRepository.InsertMany(newBookings);

            return _UnitOfWorkBooking.Save();
        }

        public IQueryable<Booking> GetAll()
        {
            return _BookingRepository.GetAll();
        }

        public IQueryable<Booking> GetActives()
        {
            return _BookingRepository.Get(
                    filter: x =>
                        x.Journeys.Any() &&
                        x.Passengers.Any() &&
                        x.Journeys.FirstOrDefault().DepartureDate >= DateTime.Today,
                    orderBy: null,
                    includeProperties: new List<System.Linq.Expressions.Expression<Func<Booking, object>>> { y => y.Passengers, z => z.Journeys },
                    page: null,
                    pageSize: null,
                    trackingEnabled: trackingStateForReading
                );
        }

        public IQueryable<Booking> GetCanceled()
        {
            return _BookingRepository.Get(
                    filter: x => !x.Journeys.Any(),
                    orderBy: null,
                    includeProperties: new List<System.Linq.Expressions.Expression<Func<Booking, object>>> 
                        {
                            y => y.Passengers, 
                            z => z.Journeys 
                        },
                    page: null,
                    pageSize: null,
                    trackingEnabled: trackingStateForReading
                );
        }

        public int DividePrices()
        {
            var bookings = GetAllBookingsForEdit();

            _BookingBulkModifierServices.DivideJourneysPrices(bookings);

            _BookingRepository.UpdateGraphs(bookings);

            return _UnitOfWorkBooking.Save();
        }

        public int ChangeFlights()
        {
            var bookings = GetAllBookingsForEdit();

            _BookingBulkModifierServices.ModifyAllItemsInBookings(bookings);

            _BookingRepository.UpdateGraphs(bookings);

            return _UnitOfWorkBooking.Save();
        }

        private List<Booking> GetAllBookingsForEdit()
        {
            var bookings = _BookingRepository.Get(null, null,
                new List<System.Linq.Expressions.Expression<Func<Booking, object>>> { y => y.Passengers, z => z.Journeys },
                null, null, trackingEnabled: trackingStateForEdit)
                .ToList();
            return bookings;
        }

    }
}
