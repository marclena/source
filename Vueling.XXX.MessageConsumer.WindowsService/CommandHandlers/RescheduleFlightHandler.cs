using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Extensions.Library;
using Vueling.Extensions.Library.DI;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Consumers.Commands;
using Vueling.XXX.Contracts.ServiceLibrary.Flights;
using Vueling.XXX.Message.Commands;
using Vueling.XXX.Message.Events.Flights;

namespace Vueling.XXX.MessageConsumer.WindowsService.CommandHandlers
{
    [RegisterService]
    public class RescheduleFlightHandler : IHandleCommand<RescheduleFlight>
    {
        private readonly IFlightReschedulerService _flightReschedulerService;

        public RescheduleFlightHandler(IFlightReschedulerService flightReschedulerService)
        {
            _flightReschedulerService = flightReschedulerService;
        }

        public void Handle(ICommandProxy proxy)
        {
            var command = proxy.GetCommand<RescheduleFlight>();

            try
            {
                // do the rescheduling
                var oldDepartureDate = _flightReschedulerService.RescheduleFlight(command.Identifier, command.NewDepartureDate);
                
                // publish a FlightRescheduled event 
                // this is not a pattern that must always be followed, but shows how commands and events can be chained
                // when an event or command is sent by a handler, the consumed command/event ConversationId is replicated to published event
                var flightRescheduled = new FlightRescheduled()
                {
                    Identifier = command.Identifier,
                    NewDepartureTime = command.NewDepartureDate,
                    OldDepartureTime = oldDepartureDate
                };

                var result = proxy.PublishEvent(flightRescheduled);


                proxy.Completed();
            }
            catch(Exception ex)
            {
                proxy.Failed(ex);
            }
        }
    }
}
