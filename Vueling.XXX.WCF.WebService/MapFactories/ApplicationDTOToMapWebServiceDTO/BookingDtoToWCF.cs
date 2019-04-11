using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.XXX.WCF.WebService.DTO;

namespace Vueling.XXX.WCF.WebService.MapFactories.ApplicationDTOToMapWebServiceDTO
{
    internal class BookingDtoToWCF : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var dto = source as Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO;

            if (dto == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new BookingResponse
            {
                Created = dto.Created,
                Id = dto.Id,
                Modified = dto.Modified,
                RecordLocator = dto.RecordLocator,
                SalesAgent = dto.SalesAgent,
                TotalJourneys = GetJourneysCount(dto.Journeys),
                TotalPassengers = GetPassengersCount(dto.Passengers)
            } as TOutput;
        }

        private int GetPassengersCount(IEnumerable<Contracts.ServiceLibrary.DTO.Passenger> passengers)
        {
            return passengers.Count();
        }

        private int GetJourneysCount(IEnumerable<Contracts.ServiceLibrary.DTO.Journey> journeys)
        {
            return journeys.Count();
        }

    }
}