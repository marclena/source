using System.Threading.Tasks;
using XX.Template.Repository.Contracts.Model;

namespace XX.Template.Repository.Contracts
{
    public interface INavitaireWcfProxy
    {
        Task<string> LogonAsync();
        Task<NavitaireWcfBookingModel> GetBookingAsync(
            string signature,
            NavitaireGetBookingRq rq);
    }
}