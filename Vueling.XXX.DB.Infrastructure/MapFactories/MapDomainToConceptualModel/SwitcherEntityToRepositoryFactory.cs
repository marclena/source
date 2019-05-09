using System;
using System.Globalization;

namespace Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel
{
    internal static class SwitcherEntityToRepositoryFactory
    {
        internal static MapDomainToConceptualModelFactoryBase GetFactoryFor(Type typeOfInputEntity, Type typeOfOutputEntity)
        {

            if (typeOfOutputEntity.FullName.Contains("AircraftRepository"))
            {
                //AircraftRepository
                switch (typeOfInputEntity.Name)
                {
                    case "Aircraft":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.AircraftRepository.AircraftEntityToAircraftDbObjectFactory();
                    case "Seat":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.AircraftRepository.SeatEntityToSeatDbObjectFactory();
                    default:
                        throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", typeOfInputEntity.Name));
                }

            }
            else if (typeOfOutputEntity.FullName.Contains("FleetRepository"))
            {
                //FleetRepository
                switch (typeOfInputEntity.Name)
                {
                    case "Aircraft":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.FleetRepository.AircraftEntityToAircraftDbObjectFactory();
                    case "Seat":
                        return new Vueling.XXX.DB.Infrastructure.MapFactories.MapDomainToConceptualModel.FleetRepository.SeatEntityToSeatDbObjectFactory();
                    default:
                        throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", typeOfInputEntity.Name));
                }
            }
            else throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "The factory for type {0} is not implemented.", typeOfInputEntity.Name));

        }

    }
}
