using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Vueling.Extensions.Library.DI;

namespace Vueling.XXX.EF.DB.Infrastructure.Repositories
{
    [RegisterServiceAttribute]
    public class BookingRepository : Vueling.DBAccess.EF.DB.Infrastructure.Repository<Vueling.XXX.Library.Entities.Booking>, Vueling.XXX.Library.InfrastructureContracts.IBookingRepository
    {
        public BookingRepository(DbContext context)
            : base(context)
        {
        }

        public override void UpdateGraph(Vueling.XXX.Library.Entities.Booking entity)
        {
            if (entity == null) { return; }

            if (Context.Entry(entity).State != EntityState.Detached)
            {
                UpdateAttachedEntity();
                return;
            }

            UpdateGraphInDetachedEntity(entity);
        }

        private void UpdateAttachedEntity()
        {
            DeleteOrphanedJourneys();
            DeleteOrphanedPassengers();
        }

        private void UpdateGraphInDetachedEntity(Vueling.XXX.Library.Entities.Booking entity)
        {
            var dbBooking = _DbSet.Where(x => x.Id == entity.Id)
                .Include(x => x.Journeys)
                .Include(x => x.Passengers)
                .SingleOrDefault();

            if (dbBooking == null) { throw new Exception(string.Format("Vueling.XXX.Library.Entities.Booking not found with Id {0}", entity.Id)); }

            UpdateBooking(entity, dbBooking);

            UpdatePassengers(entity, dbBooking);

            UpdateJourneys(entity, dbBooking);
        }

        private void UpdateBooking(Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            bool hasChanges = false;

            if (dbBooking.Created != entity.Created) { dbBooking.Created = entity.Created; hasChanges = true; }
            if (dbBooking.Modified != entity.Modified) { dbBooking.Modified = entity.Modified; hasChanges = true; }
            if (dbBooking.RecordLocator != entity.RecordLocator) { dbBooking.RecordLocator = entity.RecordLocator; hasChanges = true; }

            if (hasChanges) { Context.Entry(dbBooking).State = EntityState.Modified; }
        }

        #region Update Passengers for detached operations.

        private void UpdatePassengers(Vueling.XXX.Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            AvoidNullValuesErrorInPassengerCollection(entity, dbBooking);

            var entityPaxs = entity.Passengers.Where(x => x.Id > 0).Select(x => x.Id).ToList();
            var dbPaxs = dbBooking.Passengers.Select(x => x.Id).ToList();

            SetStateToDeletedPassengers(dbBooking, entityPaxs, dbPaxs);

            SetStateToModifiedPassengers(entity, dbBooking, entityPaxs, dbPaxs);

            SetStateToNewPassengers(entity, dbBooking);
        }

        private void SetStateToDeletedPassengers(Library.Entities.Booking dbBooking, IEnumerable<int> entityPaxs, List<int> dbPaxs)
        {
            var deletedPaxs = dbPaxs.Except(entityPaxs).ToList();

            foreach (var itemId in deletedPaxs)
            {
                var toDelete = dbBooking.Passengers.Where(x => x.Id == itemId).FirstOrDefault();
                Context.Entry(toDelete).State = EntityState.Deleted;
            }
        }

        private void SetStateToModifiedPassengers(Vueling.XXX.Library.Entities.Booking entity,
            Library.Entities.Booking dbBooking, IEnumerable<int> entityPaxs, List<int> dbPaxs)
        {
            var modifiedPaxs = entityPaxs.Intersect(dbPaxs);

            foreach (var itemId in modifiedPaxs)
            {
                var dbPax = dbBooking.Passengers.Where(x => x.Id == itemId).FirstOrDefault();

                var entityPax = entity.Passengers.Where(x => x.Id == itemId).FirstOrDefault();

                UpdatePax(entityPax, dbPax);
            }
        }

        private void SetStateToNewPassengers(Vueling.XXX.Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            AvoidNullValuesErrorInPassengerCollection(entity, dbBooking);

            foreach (var newPax in entity.Passengers.Where(x => x.Id == 0))
            {
                dbBooking.Passengers.Add(newPax);
            }
        }

        private void AvoidNullValuesErrorInPassengerCollection(Vueling.XXX.Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            if (entity.Passengers == null) { entity.Passengers = new HashSet<Vueling.XXX.Library.Entities.Passenger>(); }
            if (dbBooking.Passengers == null) { dbBooking.Passengers = new HashSet<Vueling.XXX.Library.Entities.Passenger>(); }
        }

        private void UpdatePax(Library.Entities.Passenger entity, Library.Entities.Passenger dbPax)
        {
            bool hasChanges = false;

            if (entity.FullName != dbPax.FullName) { dbPax.FullName = entity.FullName; hasChanges = true; }
            if (entity.PaxType != dbPax.PaxType) { dbPax.PaxType = entity.PaxType; hasChanges = true; }

            if (hasChanges) { Context.Entry(dbPax).State = EntityState.Modified; }
        }

        #endregion

        #region Update Journeys for detached operations.

        private void UpdateJourneys(Vueling.XXX.Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            AvoidNullValuesErrorInJourneysCollection(entity, dbBooking);

            var entityJourneys = entity.Journeys.Where(x => x.Id > 0).Select(x => x.Id).ToList();
            var dbJourneys = dbBooking.Journeys.Select(x => x.Id).ToList();

            SetStateToDeletedJourneys(dbBooking, entityJourneys, dbJourneys);

            SetStateToModifiedJourneys(entity, dbBooking, entityJourneys, dbJourneys);

            SetStateToNewJourneys(entity, dbBooking);
        }

        private void SetStateToDeletedJourneys(Library.Entities.Booking dbBooking, IEnumerable<int> entityJourneys, List<int> dbJourneys)
        {
            var deletedJourneys = dbJourneys.Except(entityJourneys).ToList();

            foreach (var itemId in deletedJourneys)
            {
                var toDelete = dbBooking.Journeys.Where(x => x.Id == itemId).FirstOrDefault();
                Context.Entry(toDelete).State = EntityState.Deleted;
            }
        }

        private void SetStateToModifiedJourneys(Library.Entities.Booking entity, Library.Entities.Booking dbBooking, IEnumerable<int> entityJourneys, List<int> dbJourneys)
        {
            var modifiedJourneys = entityJourneys.Intersect(dbJourneys);

            foreach (var itemId in modifiedJourneys)
            {
                var dbJourney = dbBooking.Journeys.Where(x => x.Id == itemId).FirstOrDefault();

                var entityJourney = entity.Journeys.Where(x => x.Id == itemId).FirstOrDefault();

                UpdateJourney(entityJourney, dbJourney);
            }
        }

        private void UpdateJourney(Library.Entities.Journey entityJourney, Library.Entities.Journey dbJourney)
        {
            bool hasChanges = false;

            if (dbJourney.Arrival != entityJourney.Arrival) { dbJourney.Arrival = entityJourney.Arrival; hasChanges = true; }
            if (dbJourney.ArrivalDate != entityJourney.ArrivalDate) { dbJourney.ArrivalDate = entityJourney.ArrivalDate; hasChanges = true; }
            if (dbJourney.Departure != entityJourney.Departure) { dbJourney.Departure = entityJourney.Departure; hasChanges = true; }
            if (dbJourney.DepartureDate != entityJourney.DepartureDate) { dbJourney.DepartureDate = entityJourney.DepartureDate; hasChanges = true; }
            if (dbJourney.Price != entityJourney.Price) { dbJourney.Price = entityJourney.Price; hasChanges = true; }

            if (hasChanges) { Context.Entry(dbJourney).State = EntityState.Modified; }
        }

        private void SetStateToNewJourneys(Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            AvoidNullValuesErrorInJourneysCollection(entity, dbBooking);

            foreach (var newJourney in entity.Journeys.Where(x => x.Id == 0))
            {
                dbBooking.Journeys.Add(newJourney);
            }
        }

        private void AvoidNullValuesErrorInJourneysCollection(Library.Entities.Booking entity, Library.Entities.Booking dbBooking)
        {
            if (entity.Journeys == null) { entity.Journeys = new HashSet<Vueling.XXX.Library.Entities.Journey>(); }
            if (dbBooking.Journeys == null) { dbBooking.Journeys = new HashSet<Vueling.XXX.Library.Entities.Journey>(); }
        }

        #endregion

        #region Delete Orphaned Entities for attached operations.

        private void DeleteOrphanedJourneys()
        {
            var bookingContext = (BoundedContexts.BookingContext)Context;

            if (bookingContext.Journeys == null) { return; }

            //delete orphaned childs
            bookingContext.Journeys
                .Local
                .Where(r => r.Booking == null)
                .ToList()
                .ForEach(r => bookingContext.Journeys.Remove(r));
        }

        private void DeleteOrphanedPassengers()
        {
            var bookingContext = (BoundedContexts.BookingContext)Context;

            if (bookingContext.Passengers == null) { return; }

            //delete orphaned childs
            bookingContext.Passengers
                .Local
                .Where(r => r.Booking == null)
                .ToList()
                .ForEach(r => bookingContext.Passengers.Remove(r));
        }

        #endregion

    }
}
