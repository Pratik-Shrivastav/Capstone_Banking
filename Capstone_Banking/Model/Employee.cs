namespace Capstone_Banking.Model
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public decimal Salary { get; set; }
        public DateTime CreatedAt { get; set; }
        public AccountDetails AccountDetailsObject { get; set; }
    }

}
