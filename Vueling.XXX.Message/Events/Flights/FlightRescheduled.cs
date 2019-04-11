using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Messaging.Message;

namespace Vueling.XXX.Message.Events.Flights
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1001:GlobalUseDecoratedServices")]
    public class FlightRescheduled : IEvent
    {
        public string Identifier { get; set; }
        public DateTime OldDepartureTime { get; set; }
        public DateTime NewDepartureTime { get; set; }
    }
}
