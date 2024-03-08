using Cubica.Models.Model;
using Cubica.Models.ViewModel;

namespace Cubica.Core.IServices
{
    public interface ICubicalBookingService
    {
        Task<CubicalBookingResult> BookCubical(CubicalBooking booking);
        Task<IEnumerable<CubicalBooking>> GetAllBooking();
    }
}
