using Cubica.Data.Repository.IRepository;
using Cubica.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace Cubica.Data.Repository
{
    public class CubicalRepository : ICubicalRepository
    {
        private readonly ApplicationDbContext _context;

        public CubicalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cubical>> GetAll()
        {
            return await _context.Cubicals.ToListAsync();
        }
    }
}
