using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        [StringLength(50, ErrorMessage = "Role can't be longer than 50 characters.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username can't be longer than 50 characters.")]
        public string UserName { get; set; }

        public DateTime CreatedOn { get; set; }

        public Bank? BankObject { get; set; }

        public Client? ClientObject { get; set; }

        public ICollection<AuditLog>? AuditLogList { get; set; } = new List<AuditLog>();

        public double? OTP {  get; set; }

        public bool? isVerified { get; set; }

    }
}
