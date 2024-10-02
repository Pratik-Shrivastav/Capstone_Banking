using Capstone_Banking.Model;

namespace Capstone_Banking.Dto
{
    public class SalaryDisbursementResponseDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
        public DateTime ProcessedAt { get; set; }
        public ICollection<Employee> EmployeeList { get; set; }
        public ICollection<Transactions>? TransactionList { get; set; }
    }
}
