using System.ComponentModel.DataAnnotations;

namespace Cubica.Models.Model
{
    public class CubicalBooking: CubicalBookingBase
    {
        [Key]
        public int BookingId { get; set; }
        public int CubicalId { get; set; }
    }
}
