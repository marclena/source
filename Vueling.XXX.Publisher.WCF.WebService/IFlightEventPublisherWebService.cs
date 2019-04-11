using System;
using System.ServiceModel;

namespace Vueling.XXX.Publisher.WCF.WebService
{
    [ServiceContract]
    public interface IFlightEventPublisherWebService
    {

        [OperationContract]
        bool PublishFlightCancelled(string flightIdentifier, DateTime cancelledDate, string cancellationReason, string cancelledBy);
    }
}
