using System;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.Publisher.Contracts.ServiceLibrary
{
    public interface IRescheduleFlightPublisherService
    {
        bool SendRescheduleFlightCommand(string flightIdentifier, DateTime newDepartureDate);
    }
}
