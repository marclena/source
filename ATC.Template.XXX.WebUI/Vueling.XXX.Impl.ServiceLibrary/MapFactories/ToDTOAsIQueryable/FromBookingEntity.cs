using System.Linq;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Impl.ServiceLibrary.MapFactories.ToDTOAsIQueryable
{
    internal class FromBookingEntity
    {
        internal IQueryable<BookingDTO> GetCollection(IQueryable<Vueling.XXX.Library.Entities.Booking> entities)
        {
            return entities.Select(u => new BookingDTO
            {
                Created = u.Created,
                Id = u.Id,
                Journeys = u.Journeys.Select(j =>
                    new Journey
                    {
                        Arrival = j.Arrival,
                        ArrivalDate = j.ArrivalDate,
                        BookingId = j.BookingId,
                        Departure = j.Departure,
                        DepartureDate = j.DepartureDate,
                        Id = j.Id,
                        Price = j.Price
                    }),
                Modified = u.Modified,
                Passengers = u.Passengers.Select(p =>
                    new Passenger
                    {
                        FullName = p.FullName,
                        BookingId = p.BookingId,
                        Id = p.Id,
                        PaxType = (Passenger.PassengerType)p.PaxType
                    }),
                RecordLocator = u.RecordLocator,
                SalesAgent = u.SalesAgent,
                TotalPrice = u.Journeys.Any() ? u.Journeys.Sum(x => x.Price) : 0
            });
        }

    }

}