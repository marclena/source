using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.InfrastructureContracts;
using Vueling.XXX.Library.Validations;

namespace Vueling.XXX.Library.DomainServicesImplementations
{
    [RegisterServiceAttribute]
    public class BookingCancelerDomainServices : Vueling.XXX.Library.DomainServicesContracts.IBookingCancelerDomainServices
    {
        /*
         * Scene using context with lazyloading disabled, tracking disabled and work with root entities.
         */

        //in this case, i will to delete directly on table Journey
        const bool trackingStateForEdit = true;

        private readonly IUnitOfWorkBookingCanceler _UnitOfWorkBookingCanceler;
        private readonly Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Booking> _BookingRepository;
        private readonly Vueling.DBAccess.Contracts.ServiceLibrary.IRepository<Journey> _JourneyRepository;

        private readonly IBookingCancelerValidations _BookingCancelerValidations;

        public BookingCancelerDomainServices(IUnitOfWorkBookingCanceler _IUnitOfWorkBookingCanceler, IBookingCancelerValidations _IBookingCancelerValidations)
        {
            _UnitOfWorkBookingCanceler = _IUnitOfWorkBookingCanceler;
            _BookingRepository = _UnitOfWorkBookingCanceler.GetRepository<Booking>();
            _JourneyRepository = _UnitOfWorkBookingCanceler.GetRepository<Journey>();

            _BookingCancelerValidations = _IBookingCancelerValidations;
        }

        public int CancelBooking(int bookingId)
        {
            var booking = GetValidBooking(bookingId);

            _BookingCancelerValidations.Validate(booking);

            RemoveJourneys(booking);

            UpdateModificationDate(booking);

            return _UnitOfWorkBookingCanceler.Save();
        }

        private Booking GetValidBooking(int bookingId)
        {
            return _BookingRepository.Get(
                    filter: x => x.Id == bookingId,
                    orderBy: null,
                    includeProperties:
                        //needed if lazyloading disabled and childs are required
                        new List<System.Linq.Expressions.Expression<Func<Booking, object>>> 
                            {
                                z => z.Journeys 
                            },
                    page: null,
                    pageSize: null,
                    trackingEnabled: trackingStateForEdit
                ).SingleOrDefault();
        }

        private void RemoveJourneys(Booking booking)
        {
            foreach (var journey in booking.Journeys.ToList())
            {
                _JourneyRepository.Delete(journey);
            }
        }

        private void UpdateModificationDate(Booking booking)
        {
            booking.Modified = DateTime.UtcNow;

            _BookingRepository.UpdateRootEntity(booking);
        }

    }
}
