using System.Globalization;
using Capstone_Banking.AutoMapperClass;
using Capstone_Banking.Data;
using Capstone_Banking.Model;
using CsvHelper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Capstone_Banking.CommonFunction
{
    public class EmployeesCsv
    {
        private static BankingDbContext _bankingDbContext = new BankingDbContext();

        public static void AddBulkEmpoloyees(int userId, IFormFile file)
        {
            User user = _bankingDbContext.UserTable.Include(c=>c.ClientObject).
                ThenInclude(e=>e.EmployeeList).FirstOrDefault(f=>f.Id == userId);
            try
            {

                var employeesList = new List<Employee>();
                using (var stream = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(stream, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<EmployeeMap>();
                    employeesList = csv.GetRecords<Employee>().ToList();
                }
                foreach (var employee in employeesList)
                {
                    employee.CreatedAt = DateTime.Now;
                    employee.IsActive = true;
                    user.ClientObject.EmployeeList.Add(employee);
                }
                _bankingDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                {
                    throw new Exception("Error Adding Employees");
                }
            }

        }
    }
}
