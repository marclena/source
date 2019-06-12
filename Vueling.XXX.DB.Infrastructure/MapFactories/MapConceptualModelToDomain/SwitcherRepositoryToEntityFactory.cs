using System;
using System.Globalization;

namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain
{
    internal static class SwitcherRepositoryToEntityFactory
    {
        internal static MapConceptualModelToDomainFactoryBase GetFactoryFor(Type typeOfdbObject)
        {

            if (typeOfdbObject.FullName.Contains("AircraftRepository"))
            {
                //AircraftRepository
                switch (typeOfdbObject.Name)
                {
                    case "Aircraft":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain.AircraftRepository.AircraftDbObjectToAircraftEntityFactory();
                    case "Seat":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain.AircraftRepository.SeatDbObjectToSeatEntityFactory();
                    default:
                        throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", typeOfdbObject.Name));
                }

            }
            else if (typeOfdbObject.FullName.Contains("FleetRepository"))
            {
                //FleetRepository
                switch (typeOfdbObject.Name)
                {
                    case "Aircraft":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain.FleetRepository.AircraftDbObjectToAircraftEntityFactory();
                    case "Seat":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapConceptualModelToDomain.FleetRepository.SeatDbObjectToSeatEntityFactory();
                    default:
                        throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", typeOfdbObject.Name));
                }
            }
            else throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", typeOfdbObject.Name));

        }
    }
}
