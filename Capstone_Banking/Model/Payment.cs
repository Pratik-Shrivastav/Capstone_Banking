using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace Capstone_Banking.Model
{
    public class Payment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Payment Type is required.")]
        [StringLength(50, ErrorMessage = "Payment Type must be at most 50 characters long.")]
        public string PaymentType { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must be at most 20 characters long.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Creation date is required.")]
        public DateTime CreatedAt { get; set; }

        [Required(ErrorMessage = "Approved By is required.")]
        public int ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public ICollection<Transactions>? Transactions { get; set; } = new List<Transactions>();
    }

}
