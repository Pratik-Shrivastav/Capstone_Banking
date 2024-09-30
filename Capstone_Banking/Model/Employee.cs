namespace Capstone_Banking.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }
        public int ClientId { get; set; }  
        public DateTime CreatedAt { get; set; }
       // public ICollection<SalaryDisbursement> SalaryDisbursements { get; set; }  
    }

}
