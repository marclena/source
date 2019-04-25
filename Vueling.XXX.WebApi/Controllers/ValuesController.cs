using Microsoft.Web.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Vueling.XXX.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/values")]
    public class ValuesController : ApiController
    {
        // GET api/values
        [Route]
        public async Task<IHttpActionResult> Get()
        {
            await Task.Run(() => { });
            return Ok(new string[] { "value1", "value2" });
        }


        // GET api/values/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            await Task.Run(() => { });
            return Ok("value");
        }

        // POST api/values
        [Route]
        public async Task<IHttpActionResult> Post([FromBody]string value)
        {
            await Task.Run(() => { });
            return Ok();
        }

        // PUT api/values/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> Put(int id, [FromBody]string value)
        {
            await Task.Run(() => { });
            return Ok();
        }

        // DELETE api/values/5
        public async Task<IHttpActionResult> Delete(int id)
        {
            await Task.Run(() => { });
            return Ok();
        }
    }
}
