using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class SalaryDisbursement
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status must be at most 20 characters long.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Processed date is required.")]
        public DateTime ProcessedAt { get; set; }

        public ICollection<SalaryFor> SalaryForList { get; set; } = new List<SalaryFor>();

        public ICollection<Transactions>? TransactionList { get; set; } = new List<Transactions>();
    }
}
