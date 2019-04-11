using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vueling.Messaging.Message;

namespace Vueling.XXX.Message.Events.Flights
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CustomRules.Maintenability", "VY1001:GlobalUseDecoratedServices")]
    public class FlightCancelled : IEvent
    {
        public string Identifier { get; set; }
        public DateTime DepartureTime { get; set; }
        public string CancellationReason { get; set; }
        public string CancelledBy { get; set; }
    }
}
