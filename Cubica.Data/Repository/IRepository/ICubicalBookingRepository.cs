using Cubica.Models.Model;

namespace Cubica.Data.Repository.IRepository
{
    public interface ICubicalBookingRepository
    {
        Task Book(CubicalBooking booking);
        Task<IEnumerable<CubicalBooking>> GetAll(DateTime? dateTime);
    }
}
