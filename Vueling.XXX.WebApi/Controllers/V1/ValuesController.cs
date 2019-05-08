using ATC.Swagger.Standard.Extension.Attributes;
using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Vueling.Maestros.Settings.Generic.Contracts.ServiceLibrary;
using Vueling.ProgrammingInterface.Contracts.ServiceLibrary;
using Vueling.ProgrammingInterface.Impl.ServiceLibrary;
using Vueling.ProgrammingInterface.Infrastructure.BookingProxy;
using Vueling.ProgrammingInterface.ServiceLibrary;

namespace Vueling.XXX.WebAPI.Controllers.V1
{
    /// <summary>
    /// Values controller v2 description
    /// </summary>
    //[Authorize]
    [Header("Accept-Language", "culture info")]
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/values")]
    public class ValuesController : ApiController
    {
        private readonly IGenericSettingsServiceContract _genericSettingsServiceContract;
        private readonly ProgrammingInterface.ServiceLibrary.INavitaireManagerFactory _navitaireManagerFactory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericSettingsServiceContract"></param>
        /// <param name="navitaireManagerFactory"></param>
        public ValuesController(IGenericSettingsServiceContract genericSettingsServiceContract,
                                ProgrammingInterface.ServiceLibrary.INavitaireManagerFactory navitaireManagerFactory)
        {
            _genericSettingsServiceContract = genericSettingsServiceContract;
            _navitaireManagerFactory = navitaireManagerFactory;
        }
        /// <summary>
        /// Get header values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route]
        [Header("Accept-Language", "method data")]
        [Header("OtherHeader", "OtherData")]
        [ResponseType(typeof(IEnumerable<string>))]
        public async Task<IHttpActionResult> Get()
        {
            var settingValueFromKey = _genericSettingsServiceContract.GetGenericSetting("VY_Skysales_services", "AMS_services").Value.ToString();

            MakeSomethingWithNavitaire();

            var result = await Task.Run(() => settingValueFromKey);

            //var result = await Task.Run(() => GetHeaderValues());
            return Ok(result);
        }


        private void MakeSomethingWithNavitaire()
        {
            INavitaireSession session = GetSession("airportsagent", "BXBrp64Sp!w98#");

            IBookingManager bookingManager = _navitaireManagerFactory.CreateManager<IBookingManager>();
            
            var resultBooking = bookingManager.GetBooking(new GetBookingRequest
            {
                Signature = session.Signature,
                ContractVersion = session.ContractVersion,
                GetBookingReqData = new GetBookingRequestData
                {
                    GetByRecordLocator = new GetByRecordLocator
                    {
                        RecordLocator = "VY12345"
                    }
                }
            });

        }

        private INavitaireSession GetSession(string agentName, string password)
        {
            var endPoint = new ManagerEndPointInformation
            {
                AgentName = agentName, // "APITest"
                Password = password, //"P@ssw0rd"
                ContractVersion = 340,
                DomainName = "DEF",
                ServerName = "https://vytestr4xapi.navitaire.com",
                ServerName4x = "https://vytestr4xapi.navitaire.com",
                Map3xTo4x = true,
                ContractVersion4x = 420,
                MaxReceivedMessageSize = 1006000
            };

            var navitaireSessionFactory = new NavitaireSessionFactory(endPoint);
            var navitaireSession = navitaireSessionFactory.CreateNavitaireSession();

            return navitaireSession;
        }


        /// <summary>
        /// Demo of getting a custom header injected by swagger due to he HeaderAttribute specification
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetHeaderValues()
        {
            return Request.Headers.GetValues("Accept-Language");
        }


        /// <summary>
        /// Create new element
        /// </summary>
        /// <param name="value">Test dto</param>
        /// <remarks>
        /// Note that the key is a GUID and not an integer.
        ///  
        ///     POST /Todo
        ///     {
        ///        "id": "0e7ad584-7788-4ab1-95a6-ca0a5b444cbb",
        ///        "name": "Item1",
        ///        "data": "31/12/2016"
        ///     }
        /// 
        /// </remarks>
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            await Task.Run(() => { });
            return Ok();
        }

        /// <summary>
        /// Put action description
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [Authorize]
        [Route("{id:int}")]
        [HttpPut]
        public async Task Put(int id, [FromBody]string value)
        {
            await Task.Run(() => { });
        }

        /// <summary>
        /// Delete action description
        /// </summary>
        /// <param name="id"></param>
        [Route("{id:int}")]
        [HttpDelete]
        public async Task Delete(int id)
        {
            await Task.Run(() => { });
        }
    }
}
