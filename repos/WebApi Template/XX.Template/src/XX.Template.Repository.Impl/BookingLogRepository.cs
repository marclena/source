using System.Threading.Tasks;
using XX.Template.Repository.Contracts;
using XX.Template.Repository.Contracts.Model;
using XX.Template.Repository.Impl.Context;
using XX.Template.Repository.Impl.Mapper.Extension;

namespace XX.Template.Repository.Impl
{
    public class BookingLogRepository : IBookingLogRepository
    {
        private readonly FooDatabaseContext _context;

        public BookingLogRepository(FooDatabaseContext context)
        {
            _context = context;
        }

        public void Add(BookingLogModel bookingLogModel)
        {
            var bookingLog = bookingLogModel.ToEntity();
            _context.BookingLogs.Add(bookingLog);
        }

        public async Task<BookingLogModel> GetByIdAsync(int id)
        {
            var bookingLog = await _context.BookingLogs.FindAsync(id);
            return bookingLog.ToModel();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}