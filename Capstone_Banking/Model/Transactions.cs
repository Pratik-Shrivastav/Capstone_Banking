namespace Capstone_Banking.Model
{
    public class Transactions
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TransactionAmount { get; set; }
        public string TransactionStatus { get; set; }
    }
}
