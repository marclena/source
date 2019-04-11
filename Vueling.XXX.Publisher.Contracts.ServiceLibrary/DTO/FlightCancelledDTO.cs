using System;

namespace Vueling.XXX.Publisher.Contracts.ServiceLibrary.DTO
{
    public class FlightCancelledDTO
    {
        public string Identifier { get; set; }
        public DateTime CancelledDate { get; set; }
        public string CancellationReason { get; set; }
        public string CancelledBy { get; set; }
    }
}
