using Cubica.Models.Model;

namespace Cubica.Models.ViewModel
{
    public class CubicalBookingResult : CubicalBookingBase
    {
        public CubicalBookingCode Code { get; set; }
        public int BookingId { get; set; }
    }
}
