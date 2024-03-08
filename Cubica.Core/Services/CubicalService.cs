using Cubica.Core.IServices;
using Cubica.Data.Repository.IRepository;
using Cubica.Models.Model;

namespace Cubica.Core.Services
{
    public class CubicalService : ICubicalService
    {
        private readonly ICubicalRepository _cubicalRepository;

        public CubicalService(ICubicalRepository cubicalRepository)
        {
            _cubicalRepository = cubicalRepository;
        }

        public async Task<IEnumerable<Cubical>> GetAll()
        {
            return await _cubicalRepository.GetAll();
        }
    }
}
