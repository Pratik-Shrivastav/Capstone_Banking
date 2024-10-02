using System.Transactions;

namespace Capstone_Banking.Model
{
    public class Payment
    {
        public int Id { get; set; }

        public string PaymentType { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public int ApprovedBy { get; set; }  
        public DateTime? ApprovedAt { get; set; }
        public ICollection<Transactions>? Transactions { get; set; } = new List<Transactions>();
    }

}
