using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Vueling.Extensions.Library.DI;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Publishers.Commands;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Publishers.Common.Tracking;
using Vueling.XXX.Message.Commands;
using Vueling.XXX.Publisher.Contracts.ServiceLibrary;

namespace Vueling.XXX.Publisher.Impl.ServiceLibrary
{
    [RegisterService]
    public class RescheduleFlightPublisherService : IRescheduleFlightPublisherService
    {
        private readonly ICommandSender _commandSender;

        public RescheduleFlightPublisherService(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public bool SendRescheduleFlightCommand(string flightIdentifier, DateTime newDepartureDate)
        {
            var rescheduleFlight = new RescheduleFlight()
            {
                Identifier = flightIdentifier,
                NewDepartureDate = newDepartureDate
            };

            var result = _commandSender.SendCommand("XXX-Example-MessageConsumer", rescheduleFlight);
            if(result.Status != SendStatus.Success)
            {
                Trace.TraceError(string.Format("RescheduleFlight command not sent. Identifier {0}. Status: {1}, Description: {2}",
                    rescheduleFlight.Identifier, result.Status, result.Description));
                return false;
            }

            return true;
        }
    }
}
