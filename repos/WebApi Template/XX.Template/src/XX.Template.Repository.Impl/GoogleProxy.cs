using System.Net.Http;
using System.Threading.Tasks;
using XX.Template.Repository.Contracts;

namespace XX.Template.Repository.Impl
{
    //Not in use. It's an example about how to create httpclient proxies with the ATC.HttpClientFactory Library
    public class GoogleProxy: IGoogleProxy
    {
        private readonly HttpClient _client;

        public GoogleProxy(HttpClient client)
        {
            _client = client;
        }

        public async Task InvokeAsync()
        {
            await _client.GetAsync("/analytics/optimize");
        }
    }
}
