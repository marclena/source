using System.Collections.Generic;
using System.Linq;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;
using Vueling.XXX.Impl.ServiceLibrary.MapFactories.MapDomainToDTO;

namespace Vueling.XXX.Impl.ServiceLibrary.Implementations
{
    [RegisterServiceAttribute]
    public class BookingApplicationServices : Vueling.XXX.Contracts.ServiceLibrary.IBookingApplicationServices
    {
        private readonly Vueling.XXX.Library.DomainServicesContracts.IBookingDomainServices _BookingDomainServices;

        public BookingApplicationServices(Vueling.XXX.Library.DomainServicesContracts.IBookingDomainServices _IBookingDomainServices)
        {
            _BookingDomainServices = _IBookingDomainServices;
        }

        public int CreateBooking(int amount)
        {
            return _BookingDomainServices.CreateSampleBooking(amount);
        }

        public List<BookingDTO> GetActives()
        {
            var bookings = _BookingDomainServices.GetActives().ToList();

            return GetMappedBookings(bookings);
        }

        public IQueryable<BookingDTO> GetAll()
        {
            var bookings = _BookingDomainServices.GetAll();

            return GetMappedAsQueryable(bookings);
        }

        public int GetActivesCount()
        {
            return _BookingDomainServices.GetActives().Count();
        }

        public List<BookingDTO> GetCanceled()
        {
            var bookings = _BookingDomainServices.GetCanceled().ToList();

            return GetMappedBookings(bookings);
        }

        public List<BookingDTO> GetActivesByPages(int page, int pageSize)
        {
            var bookings = _BookingDomainServices.GetActives().OrderBy(x => x.Id).Skip(page - 1).Take(page * pageSize).ToList();

            return GetMappedBookings(bookings);
        }

        public List<BookingDTO> GetCanceledByPages(int page, int pageSize)
        {
            var bookings = _BookingDomainServices.GetCanceled().OrderBy(x => x.Id).Skip(page - 1).Take(page * pageSize).ToList();

            return GetMappedBookings(bookings);
        }

        public int ChangeFlights()
        {
            return _BookingDomainServices.ChangeFlights();
        }

        public int DividePrices()
        {
            return _BookingDomainServices.DividePrices();
        }

        private List<BookingDTO> GetMappedBookings(List<Vueling.XXX.Library.Entities.Booking> entities)
        {
            var mapping = MappingFromDomainFactory.GetFor(DomainToDtoEnum.Booking);

            return mapping.GetCollection<Vueling.XXX.Library.Entities.Booking, BookingDTO>(entities).ToList();
        }

        private IQueryable<BookingDTO> GetMappedAsQueryable(IQueryable<Vueling.XXX.Library.Entities.Booking> entities)
        {
            return new Vueling.XXX.Impl.ServiceLibrary.MapFactories.ToDTOAsIQueryable.FromBookingEntity()
                .GetCollection(entities);
        }
    }
}
