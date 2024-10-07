using Capstone_Banking.Model;
using CsvHelper.Configuration;

namespace Capstone_Banking.AutoMapperClass
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap() 
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Email).Name("Email");
            Map(m => m.Designation).Name("Designation");
            Map(m => m.Salary).Name("Salary");

            Map(m => m.AccountDetailsObject.AccountNumber).Name("AccountNumber");
            Map(m => m.AccountDetailsObject.IFSC).Name("IFSC");
            Map(m => m.AccountDetailsObject.Branch).Name("Branch");

        }
    }
}
