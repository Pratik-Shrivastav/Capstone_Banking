using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class AccountDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)] // Adjust the length based on your requirements
        public string AccountNumber { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC format.")]
        public string IFSC { get; set; }

        [StringLength(50)] // Adjust the length based on your requirements
        public string Branch { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Account balance must be a non-negative value.")]
        public double? AccountBalance { get; set; }
    }
}
