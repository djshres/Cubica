using Cubica.Data.Repository.IRepository;
using Cubica.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace Cubica.Data.Repository
{
    public class CubicalBookingRepository : ICubicalBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public CubicalBookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Book(CubicalBooking booking)
        {
            await _context.CubicalBookings.AddAsync(booking);
            _context.SaveChanges();
        }

        public async Task<IEnumerable<CubicalBooking>> GetAll(DateTime? date)
        {
            if (date != null)
            {
                return await _context.CubicalBookings.Where(x => x.Date == date).OrderBy(x => x.BookingId).ToListAsync();
            }
            return await _context.CubicalBookings.OrderBy(x => x.BookingId).ToListAsync();
        }
    }
}
