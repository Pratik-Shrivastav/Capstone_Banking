namespace Capstone_Banking.Dto
{
    namespace Capstone_Banking.Model
    {
        public class TransactionDto
        {
            public int Id { get; set; } // Transaction ID
            public DateTime TransactionDate { get; set; } // Date of the transaction
            public double TransactionAmount { get; set; } // Amount involved in the transaction
            public string TransactionStatus { get; set; } // Status of the transaction (e.g., Pending, Success)
            public int? EmployeePaidId { get; set; } // ID of the employee who processed the payment, if applicable
        }
    }

}
