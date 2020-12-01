using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.DomainServicesContracts;
using Vueling.XXX.Library.Entities;
using Vueling.XXX.Library.Exceptions;

namespace Vueling.XXX.Library.Validations
{
    [RegisterServiceAttribute]
    public class BookingCancelerValidations : Vueling.XXX.Library.Validations.IBookingCancelerValidations
    {
        private readonly IBookingFeaturesDomainServices _BookingFeaturesDomainServices;

        public BookingCancelerValidations(IBookingFeaturesDomainServices _IBookingFeaturesDomainServices)
        {
            _BookingFeaturesDomainServices = _IBookingFeaturesDomainServices;
        }

        public void Validate(Booking booking)
        {
            ValidateBookingNull(booking);
            ValidateJourneysNull(booking);
            ValidateBookingIsAlreadyFlew(booking);
            ValidateBookingIsAgency(booking);
        }

        private void ValidateBookingNull(Booking booking)
        {
            if (booking == null) { throw new InvalidBookingOperationException("Booking is null."); }
        }

        private void ValidateJourneysNull(Booking booking)
        {
            if (!booking.Journeys.Any()) { throw new InvalidBookingOperationException("Journeys can't be null. Maybe it was canceled previously."); }
        }

        private void ValidateBookingIsAlreadyFlew(Booking booking)
        {
            if (booking.IsAlreadyFlew()) { throw new InvalidBookingOperationException("Booking flew."); }
        }

        private void ValidateBookingIsAgency(Booking booking)
        {
            if (_BookingFeaturesDomainServices.IsAgency(booking)) { throw new InvalidBookingOperationException("This booking was created by some Agency."); }
        }

    }
}
