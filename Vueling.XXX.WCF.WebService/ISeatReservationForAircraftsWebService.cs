using System;
using System.ServiceModel;

namespace Vueling.XXX.WCF.WebService
{

    [ServiceContract]
    public interface ISeatReservationForAircraftsWebService
    {

        [OperationContract]
        string ReserveASeat(string flighIdentifier, DateTime departureTime, int row, string colum);

        [OperationContract]
        string ChangeASeatReservation(string flighIdentifier, DateTime departureTime, int currentRow, string currentColum, int newRow, string newColum);

    }

}
