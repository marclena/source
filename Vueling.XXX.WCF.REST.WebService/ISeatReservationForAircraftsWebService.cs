using System.ServiceModel;
using System.ServiceModel.Web;

namespace Vueling.XXX.WCF.REST.WebService
{
    [ServiceContract]
    public interface ISeatReservationForAircraftsWebService
    {
        [OperationContract]
        [WebGet(
            ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "/ReserveASeat/{flighIdentifier}/{departureTimeString}/{row}/{colum}"
            )
        ]
        string ReserveASeat(string flighIdentifier, string departureTimeString, string row, string colum);

    }
}
