using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vueling.XXX.DB.Infrastructure.UnitTest")]
namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.AircraftRepository
{
    class AircraftEntityToAircraftDbObjectFactory : MapDomainToConceptualModelFactoryBase
    {

        internal override TOutput GetDbObjectFromEntity<TInput, TOutput>(TInput entity)
        {

            var aircraft = entity as Vueling.XXX.Library.Entities.Aircraft;//if casting fail, return null, so we have to verify the result
            if (aircraft == null) { throw new InvalidCastException("Cast to type Vueling.XXX.Library.Entities.Aircraft has fail."); }

            Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository.Aircraft dbAircraftObject = new Vueling.XXX.DB.Infrastructure.Repositories.AircraftRepository.Aircraft();

            dbAircraftObject.FlightNumber = aircraft.FlightNumber;
            dbAircraftObject.DepartureDate = aircraft.DepartureDate;
            dbAircraftObject.Seats = string.Empty;
            dbAircraftObject.BusySeats = string.Empty;

            foreach(var item in aircraft.Seats)
            {
                dbAircraftObject.Seats += "," + item.Row + item.Column;
                if (item.IsAvailable == false) dbAircraftObject.BusySeats += "," + item.Row + item.Column;
            }
            if (dbAircraftObject.Seats.Length > 0) dbAircraftObject.Seats = dbAircraftObject.Seats.Remove(0, 1);
            if(dbAircraftObject.BusySeats.Length > 0) dbAircraftObject.BusySeats = dbAircraftObject.BusySeats.Remove(0, 1);

            dbAircraftObject.LastUpdate = DateTime.Now;

            return (dbAircraftObject) as TOutput;
        }

    }
}
