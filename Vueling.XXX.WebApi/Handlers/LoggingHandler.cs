using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Vueling.XXX.WebAPI.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggingHandler : DelegatingHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Trace.TraceInformation("[{0}] Begin action.", request.RequestUri.ToString());
            return await base.SendAsync(request, cancellationToken).ContinueWith((task) => 
            {               
                var response = task.Result;
                if (!response.IsSuccessStatusCode)
                {
                    string errorText;
                    try
                    {
                        errorText = response.Content.ReadAsStringAsync().Result;
                    }
                    catch (Exception)
                    {
                        errorText = "<ERROR_RESOLVING_RESPONSE_CONTENT>";
                    }
                    Trace.TraceError("[{0}] Error: {1}.", request.RequestUri.ToString(), errorText);
                }
                Trace.TraceInformation("[{0}] End action.", request.RequestUri.ToString());
                return response;
            });
        }
    }
}