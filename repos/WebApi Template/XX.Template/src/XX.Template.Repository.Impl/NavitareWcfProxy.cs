using System.Threading.Tasks;
using XX.Template.Repository.Contracts;
using XX.Template.Repository.Contracts.Model;
using XX.Template.Repository.Impl.Configuration.Options;
using XX.Template.Repository.Impl.Mapper.Extension;
using ExternalServiceName.ExternalService.BookingManager;
using ExternalServiceName.ExternalService.SessionManager;
using Microsoft.Extensions.Options;

namespace XX.Template.Repository.Impl
{
    public class NavitareWcfProxy: INavitaireWcfProxy
    {
        private readonly IBookingManager _bookingManager;
        private readonly ISessionManager _sessionManager;
        private readonly NavitaireParametersOptions _navitaireParameters;


        public NavitareWcfProxy(IBookingManager bookingManager, 
            ISessionManager sessionManager,
            IOptions<NavitaireParametersOptions> navitaireOptions)
        {
            _bookingManager = bookingManager;
            _sessionManager = sessionManager;
            _navitaireParameters = navitaireOptions.Value;
        }

        public async Task<string> LogonAsync()
        {
            var login =  await _sessionManager.LogonAsync(new LogonRequest
            {
                logonRequestData = new LogonRequestData
                {
                    DomainCode = _navitaireParameters.NavitaireLogin.DomainCode,
                    AgentName = _navitaireParameters.NavitaireLogin.AgentName,
                    Password = _navitaireParameters.NavitaireLogin.Password
                }
            });

            return login?.Signature;
        }

        public async Task<NavitaireWcfBookingModel> GetBookingAsync(
            string signature, 
            NavitaireGetBookingRq rq)
        {
            var rqt = new GetBookingRequest
            {
                Signature = signature,
                GetBookingReqData = rq.MapToGetBookingRequestData()
            };

            var bookingRs =  await _bookingManager.GetBookingAsync(rqt);

            return bookingRs.MapToNavitaireWcfBookingModel();
        }
    }
}
