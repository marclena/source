using Microsoft.Web.Http;
using System.Web.Http;

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
        public IHttpActionResult Get()
        {

            return Ok(new string[] { GetApiKey(), GetCulture() });
        }

        private string GetCulture()
        {
            return "cultureV2";
        }

        private string GetApiKey()
        {
            return "apiKeyv2";
        }




        ///// <summary>
        ///// Get by id
        ///// </summary>
        ///// <param name="id">id dto</param>
        ///// <response code="200">Returns a list of item</response>
        //[Route("{id:int}")]        
        //[ResponseType(typeof(IEnumerable<TestDto>))]
        //public IHttpActionResult Get(int id)
        //{
        //    var result = new List<TestDto>();
        //    return Ok(result);
        //}

        ///// <summary>
        ///// Post action description
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[Authorize]
        //[HttpPost]
        //[Route]
        //public IHttpActionResult Post([FromBody]TestDto value)
        //{

        //    return Ok();
        //}
    }
}
