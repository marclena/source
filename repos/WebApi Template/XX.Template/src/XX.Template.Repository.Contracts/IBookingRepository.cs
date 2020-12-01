using System.Threading.Tasks;
using XX.Template.Repository.Contracts.Model;

namespace XX.Template.Repository.Contracts
{
    public interface IBookingLogRepository
    {
        void Add(BookingLogModel bookingLog);

        Task<BookingLogModel> GetByIdAsync(int id);

        Task<int> SaveAsync();
    }
}