using Cubica.Models.ModelValidation;
using System.ComponentModel.DataAnnotations;

namespace Cubica.Models.Model
{
    public class CubicalBookingBase
    {
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public required string Email { get; set; }

        [DataType(DataType.Date)]
        [DateInFuture]
        public DateTime Date { get; set; }
    }
}
