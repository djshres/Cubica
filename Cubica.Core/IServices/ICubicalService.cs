using Cubica.Models.Model;

namespace Cubica.Core.IServices
{
    public interface ICubicalService
    {
        Task<IEnumerable<Cubical>> GetAll();
    }
}
