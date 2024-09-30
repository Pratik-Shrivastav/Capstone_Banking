namespace Capstone_Banking.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public string PaymentType { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }  // Pending, Approved, Rejected
        public DateTime CreatedAt { get; set; }
        public int ApprovedBy { get; set; }  // Foreign key to User (Bank User)
        public DateTime? ApprovedAt { get; set; }
        //public ICollection<Transaction> Transactions { get; set; }  // Relationship with Transactions
    }

}
