using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using XX.Template.Core.Extensions;
using XX.Template.Library.Contracts;
using XX.Template.Library.Contracts.Dto;
using ExternalServiceName.ExternalService.BookingManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using App.Metrics;
using System;

namespace XX.Template.WebApi.Controllers.V1
{
    /// <summary>
    ///     Get or Create booking services
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class MyServiceController : Controller
    {
        private readonly IFooService _fooService;
        private readonly IGoogleService _googleService;

        private readonly IMetrics _metrics;

        public MyServiceController(IFooService fooService,
            IGoogleService googleService, IMetrics metrics)
        {
            _fooService = fooService;
            _googleService = googleService;
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
        }

        /// <summary>
        ///     Get booking by PNR X7CERP
        /// </summary>
        /// <returns></returns>
        [HttpGet("RecordLocator/{pnr}", Name = "GetByRecordLocator")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(FooRsDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(List<ErrorResult>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByRecordLocator(string pnr)
        {

           // using (_metrics.Measure.Timer.Time(MetricsRegistry.TimerUsingExponentialForwardDecayingReservoir))
            //{

                var response = await _fooService.DoSomethingAsync(new FooRqDto { RecordLocator = pnr });
                if (!response.HasErrors)
                    return Ok(response.Result);
                if (response.Result == null &&
                    !response.HasErrors)
                    return NoContent();
                return BadRequest(response.Errors);
            //}


            //}
        }


        /// <summary>
        ///     Create booking
        /// </summary>
        /// <param name="booking"></param>
        /// <returns></returns>
        [HttpPost("Booking", Name = "CreateBooking")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.Created)]
        [ProducesResponseType(typeof(List<ErrorResult>), (int) HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int) HttpStatusCode.InternalServerError)]
        public IActionResult PostBooking([FromBody] object booking)
        {
            return CreatedAtRoute("GetByRecordLocator", new {pnr = "createBookingRecordLocator"}, booking);
        }

        [Authorize]
        [HttpGet("identity", Name = "GetIdentity")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ClaimsPrincipal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(List<ErrorResult>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetIdentity()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(HttpContext.User.Claims);
            }

            return NoContent();


        }

        /// <summary>
        ///     Invokes a call to google.com via httpclient proxy
        /// </summary>
        /// <returns></returns>
        [HttpGet("Proxy", Name = "InvokeProxy")]
        [Produces("application/json")]
        public async Task InvokeProxy()
        {
            await _googleService.InvokeAsync();
        }
        private Task Delay()
        {
            var second = DateTime.Now.Second;

            if (second <= 20)
            {
                return Task.CompletedTask;
            }

            if (second <= 40)
            {
                return Task.Delay(TimeSpan.FromMilliseconds(50), HttpContext.RequestAborted);
            }

            return Task.Delay(TimeSpan.FromMilliseconds(100), HttpContext.RequestAborted);
        }
    }
}