using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Vueling.XXX.WebApi.Controllers.V2
{

    /// <summary>
    /// 
    /// </summary>
    //[Header("Accept-Language", "culture info")]
    [ApiVersion("2.0")]
    [RoutePrefix("api/v{version:apiVersion}/values")]
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Get all elements
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> Get()
        {
            var result = await Task.Run(() => new string[] { GetApiKey(), GetCulture() });
            return Ok(result);
        }

        private string GetCulture()
        {
            return "cultureV2";
        }

        private string GetApiKey()
        {
            return "apiKeyv2";
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <param name="id">id dto</param>
        /// <response code="200">Returns a list of item</response>
        [Route("{id:int}")]
        [ResponseType(typeof(IEnumerable<string>))]
        public IHttpActionResult Get(int id)
        {
            var result = new List<string>();
            return Ok(result);
        }

        /// <summary>
        /// Post action description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            await Task.Run(() => { });
            return Ok();
        }
    }
}
