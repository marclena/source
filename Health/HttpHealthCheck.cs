using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATC.HealthCheck.NET
{
    public class HttpHealthCheck : IHealthCheck
    {
        private string _url;
        public string HealthCheckName { get; set; } 

        public HttpHealthCheck(string url)
        {
            _url = url;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync(_url);
                    if (!response.IsSuccessStatusCode)
                    {
                        return await Task.FromResult(HealthCheckResult.Unhealthy());
                        //throw new Exception("Url not responding with 200 OK");
                    }
                }
                catch (Exception)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy());
                }
            }
            return await Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
