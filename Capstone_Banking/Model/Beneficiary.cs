using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class Beneficiary
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Beneficiary Name is required.")]
        [StringLength(100, ErrorMessage = "Beneficiary Name must be at most 100 characters long.")]
        public string BenificiaryName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(200, ErrorMessage = "Email must be at most 200 characters long.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreatedOn { get; set; } = DateTime.Now; // Defaults to now if not set

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Account details are required.")]
        public AccountDetails AccountDetailsObject { get; set; }

        public ICollection<Payment>? PaymentsList { get; set; }




    }
}
