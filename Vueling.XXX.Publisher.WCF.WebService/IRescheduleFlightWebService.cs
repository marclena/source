using System;
using System.ServiceModel;

namespace Vueling.XXX.Publisher.WCF.WebService
{
    [ServiceContract]
    public interface IRescheduleFlightWebService
    {
        [OperationContract]
        bool RescheduleFlight(string flightIdentifier, DateTime newDepartureTime);
    }
}
