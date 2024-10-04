using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class Transactions
    {
        
        public int Id { get; set; }

        [Required(ErrorMessage = "Transaction date is required.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Transaction amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Transaction amount must be greater than zero.")]
        public double TransactionAmount { get; set; }

        [Required(ErrorMessage = "Transaction status is required.")]
        [StringLength(50, ErrorMessage = "Transaction status can't be longer than 50 characters.")]
        public string TransactionStatus { get; set; }

        public int? EmployeePaidId { get; set; }
    }
}
