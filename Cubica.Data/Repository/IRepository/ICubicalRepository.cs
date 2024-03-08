using Cubica.Models.Model;

namespace Cubica.Data.Repository.IRepository
{
    public interface ICubicalRepository
    {
        Task<IEnumerable<Cubical>> GetAll();
    }
}
