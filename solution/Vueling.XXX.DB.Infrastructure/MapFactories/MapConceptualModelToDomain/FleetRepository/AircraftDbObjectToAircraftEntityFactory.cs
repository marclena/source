using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Vueling.XXX.DB.Infrastructure.UnitTest")]
namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain.FleetRepository
{
    class AircraftDbObjectToAircraftEntityFactory : MapConceptualModelToDomainFactoryBase
    {

        internal override TOutput GetEntityFromDbObject<TInput, TOutput>(TInput dbobject)
        {
            if (dbobject == null) { return default(TOutput); }

            var aircraft = dbobject as Vueling.XXX.DB.Infrastructure.Repositories.FleetRepository.Aircraft;//if casting fail, return null, so we have to verify the result
            if (aircraft == null) { throw new InvalidCastException("Cast to type Vueling.XXX.Infrastructure.Repositories.FleetRepository.Aircraft has fail."); }

            var allSeats = aircraft.Seats.Split(',');

            var seats = new List<Vueling.XXX.Library.Entities.Seat>();
            foreach (var item in allSeats)
            {
                var row = System.Text.RegularExpressions.Regex.Match(item, "[0-9]+", System.Text.RegularExpressions.RegexOptions.Compiled).Value;
                var column = System.Text.RegularExpressions.Regex.Match(item.ToUpper(CultureInfo.InvariantCulture), "[A-Z]+", System.Text.RegularExpressions.RegexOptions.Compiled).Value;

                Vueling.XXX.Library.Entities.AvailabilityEnum availability;
                if (!string.IsNullOrEmpty(aircraft.BusySeats) && aircraft.BusySeats.ToUpper(CultureInfo.InvariantCulture).Contains(item.ToUpper(CultureInfo.InvariantCulture)))
                {
                    availability = Vueling.XXX.Library.Entities.AvailabilityEnum.Busy;
                }
                else { availability = Vueling.XXX.Library.Entities.AvailabilityEnum.Available; }

                seats.Add(new Vueling.XXX.Library.Entities.Seat { Availability = availability, Column = column, Row = row });
            }

            return (new Vueling.XXX.Library.Entities.Aircraft(aircraft.FlightNumber, aircraft.DepartureDate, seats)) as TOutput;
        }

    }
}
