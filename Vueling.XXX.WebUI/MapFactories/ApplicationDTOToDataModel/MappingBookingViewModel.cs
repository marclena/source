using System;
using System.Linq;
using Vueling.XXX.WebUI.Models;

namespace Vueling.XXX.WebUI.MapFactories.ApplicationDTOToDataModel
{
    internal class MappingBookingViewModel : MappingBase
    {
        internal override TOutput Get<TInput, TOutput>(TInput source)
        {
            if (source == null) { return default(TOutput); }

            var dto = source as Vueling.XXX.Contracts.ServiceLibrary.DTO.BookingDTO;

            if (dto == null) { throw new InvalidCastException(typeof(TInput).Name); }

            return new BookingViewModel
            {
                Created = dto.Created,
                Id = dto.Id,
                Modified = dto.Modified,
                RecordLocator = dto.RecordLocator,
                SalesAgent = dto.SalesAgent,
                TotalPrice = dto.TotalPrice,
                JourneysCount = dto.Journeys.Any() ? dto.Journeys.Count() : 0,
                PassengersCount = dto.Passengers.Any() ? dto.Passengers.Count() : 0,
            } as TOutput;
        }
    }
}