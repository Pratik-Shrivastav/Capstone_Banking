using System.ComponentModel.DataAnnotations;

namespace Capstone_Banking.Model
{
    public class SalaryFor
    {
        [Key]
        public int Id { get; set; }
        public int EmployeeId { get; set; }
    }
}
