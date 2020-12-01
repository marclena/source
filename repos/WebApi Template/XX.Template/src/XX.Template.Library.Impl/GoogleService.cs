using System.Threading.Tasks;
using XX.Template.Library.Contracts;
using XX.Template.Repository.Contracts;

namespace XX.Template.Library.Impl
{
    public class GoogleService: IGoogleService
    {
        private readonly IGoogleProxy _proxy;

        public GoogleService(IGoogleProxy proxy)
        {
            _proxy = proxy;
        }
        public async Task InvokeAsync()
        {
            await _proxy.InvokeAsync();
        }
    }
}
