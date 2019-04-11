using NMock;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using Vueling.XXX.Contracts.DTO.ServiceLibrary;
using Vueling.XXX.Contracts.ServiceLibrary.DTO;

namespace Vueling.XXX.WebUI.UnitTest.Helpers
{
    internal static class HttpContextBaseMockFactory
    {

        internal static Mock<HttpContextBase> GetContext(MockFactory mockfactory, FlightDTO fligh, SeatDTO seat)
        {

            NameValueCollection form = new NameValueCollection();
            form.Add("flighIdentifier", fligh.Identifier);
            form.Add("departureTimeString", fligh.DepartureTime.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture));
            form.Add("row", seat.Row);
            form.Add("column", seat.Column);

            Mock<HttpContextBase> context = mockfactory.CreateMock<HttpContextBase>();
            Mock<HttpRequestBase> request = mockfactory.CreateMock<HttpRequestBase>();
            Mock<HttpResponseBase> response = mockfactory.CreateMock<HttpResponseBase>();

            request.Expects.AtLeastOne.GetProperty(v => v.HttpMethod).WillReturn("POST");

            request.Expects.AtLeastOne.GetProperty(v => v.Form).WillReturn(form);
            context.Expects.AtLeastOne.GetProperty(v => v.Request).WillReturn(request.MockObject);
            context.Expects.AtLeastOne.GetProperty(v => v.Response).WillReturn(response.MockObject);

            return context;

        }

    }
}
