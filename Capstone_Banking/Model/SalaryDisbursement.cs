namespace Capstone_Banking.Model
{
    public class SalaryDisbursement
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
        public ICollection<SalaryFor> SalaryForList { get; set; }
        public ICollection<Transactions>? TransactionList { get; set; }
    }
}
