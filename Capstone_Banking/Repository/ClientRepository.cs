using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone_Banking.Repository
{
    public class ClientRepository : IClientRepository
    {
        private readonly BankingDbContext _bankingDbContext;

        public ClientRepository(BankingDbContext bankingDbContext)
        {
            _bankingDbContext = bankingDbContext;
        }

        // Add Employee for specific client
        public async Task<Employee> AddEmployeeAsync(Employee employee, int userId)
        {
            var user = await _bankingDbContext.UserTable.Include(c => c.ClientObject)
                .ThenInclude(p => p.EmployeeList)
                .FirstOrDefaultAsync(c => c.Id == userId);

            employee.CreatedAt = DateTime.UtcNow;
            user.ClientObject.EmployeeList.Add(employee);
            await _bankingDbContext.SaveChangesAsync();

            return employee;
        }

        // Add Beneficiary for specific client
        public async Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary, int userId)
        {
            var user = await _bankingDbContext.UserTable.Include(c => c.ClientObject)
                .ThenInclude(p => p.BeneficiaryList)
                .FirstOrDefaultAsync(c => c.Id == userId);

            user.ClientObject.BeneficiaryList.Add(beneficiary);
            await _bankingDbContext.SaveChangesAsync();
            return beneficiary;
        }

        public async Task<ICollection<Employee>> GetAllEmployeesAsync(int userId)
        {
            User user = await _bankingDbContext.UserTable
                            .Include(x => x.ClientObject)
                            .ThenInclude(y => y.EmployeeList)
                            .ThenInclude(z => z.AccountDetailsObject)
                            .FirstOrDefaultAsync(y => y.Id == userId);

            return user.ClientObject.EmployeeList;
        }
        // Get All Employees Paged
        public async Task<(IEnumerable<Employee> employees, int totalCount)> GetEmployeesPagedAsync(int userId, int page, int pageSize)
        {
            User user = await _bankingDbContext.UserTable
                .Include(x => x.ClientObject)
                .ThenInclude(y => y.EmployeeList)
                .ThenInclude(z => z.AccountDetailsObject)
                .FirstOrDefaultAsync(y => y.Id == userId);

            // Check if user or their ClientObject is null
            if (user?.ClientObject?.EmployeeList == null)
            {
                return (Enumerable.Empty<Employee>(), 0); // Return empty if no employees found
            }

            // Get the total count of employees
            var totalCount = (user.ClientObject.EmployeeList.Where(c=>c.IsActive).ToList()).Count;

            // Apply pagination
            var paginatedEmployees = user.ClientObject.EmployeeList
                .Where(c=>c.IsActive)
                .Skip((page - 1) * pageSize) // Skip previous pages
                .Take(pageSize)              // Take the number of employees for the current page
                .ToList();


            return (paginatedEmployees, totalCount);
        }
        public async Task<ICollection<Employee>> SearchEmployeesAsync(int userId, string searchTerm)
        {
            // Fetch the client associated with the user
            var employees = await _bankingDbContext.EmployeeTable
                .Include(x => x.AccountDetailsObject).Where(s=>s.Name.StartsWith(searchTerm)).ToListAsync();
                
            return employees;
        }



        // Get All Beneficiaries
        public async Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId)
        {
            User user = await _bankingDbContext.UserTable.Include(x => x.ClientObject)
                .ThenInclude(y => y.BeneficiaryList)
                .ThenInclude(z => z.AccountDetailsObject)
                .FirstOrDefaultAsync(y => y.Id == userId);
            return user.ClientObject.BeneficiaryList;
        }

        // Get All Beneficiaries Paged
        public async Task<(IEnumerable<Beneficiary> beneficiaries, int totalCount)> GetBeneficiaryPagedAsync(int userId, int page, int pageSize)
        {
            User user = await _bankingDbContext.UserTable
                .Include(x => x.ClientObject)
                .ThenInclude(y => y.BeneficiaryList)
                .ThenInclude(z => z.AccountDetailsObject)
                .FirstOrDefaultAsync(y => y.Id == userId);

            // Check if user or their ClientObject is null
            

            // Get the total count of employees
            var totalCount = (user.ClientObject.BeneficiaryList.Where(c => c.IsActive).ToList()).Count;
            Console.WriteLine(totalCount);
            // Apply pagination
            var paginatedBeneficiaries = user.ClientObject.BeneficiaryList
                .Where(c => c.IsActive)
                .Skip((page - 1) * pageSize) // Skip previous pages
                .Take(pageSize)              // Take the number of employees for the current page
                .ToList();


            return (paginatedBeneficiaries, totalCount);
        }
        public async Task<ICollection<Beneficiary>> SearchBeneficiaryAsync(int userId, string searchTerm)
        {
            // Fetch the client associated with the user
            var beneficiaries = await _bankingDbContext.BeneficiaryTable
                .Include(x => x.AccountDetailsObject).Where(s => s.BenificiaryName.StartsWith(searchTerm)).ToListAsync();

            return beneficiaries;
        }

        // Get Employee by ID
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _bankingDbContext.EmployeeTable
                .Include(e => e.AccountDetailsObject)  // Include Account Details
                .FirstOrDefaultAsync(e => e.EmployeeId == id); // Use FirstOrDefaultAsync for better handling of nulls
        }

        // Get Beneficiary by ID
        public async Task<Beneficiary> GetBeneficiaryByIdAsync(int id)
        {
            return await _bankingDbContext.BeneficiaryTable
                .Include(b => b.AccountDetailsObject)   // Include Account Details
                .Include(b => b.PaymentsList)            // Include Payments if necessary
                .ThenInclude(p => p.Transactions)         // Include Transactions related to Payments if needed
                .FirstOrDefaultAsync(b => b.Id == id); // Use FirstOrDefaultAsync for better handling of nulls
        }

        // Update Employee
        public async Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            _bankingDbContext.EmployeeTable.Update(employee);
            await _bankingDbContext.SaveChangesAsync();
            return employee;
        }

        // Update Beneficiary
        public async Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary)
        {
            _bankingDbContext.BeneficiaryTable.Update(beneficiary);
            await _bankingDbContext.SaveChangesAsync();
            return beneficiary;
        }

        // Delete Employee
        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _bankingDbContext.EmployeeTable.FindAsync(id);
            if (employee != null)
            {
                employee.IsActive = false;  // Soft delete by marking as inactive
                _bankingDbContext.EmployeeTable.Update(employee);
                await _bankingDbContext.SaveChangesAsync();
            }
        }

        // Delete Beneficiary
        public async Task DeleteBeneficiaryAsync(int id)
        {
            var beneficiary = await _bankingDbContext.BeneficiaryTable.FindAsync(id);
            if (beneficiary != null)
            {
                beneficiary.IsActive = false;  // Soft delete by marking as inactive
                _bankingDbContext.BeneficiaryTable.Update(beneficiary);
                await _bankingDbContext.SaveChangesAsync();
            }
        }

        // Salary Disbursement for a specific client
        public async Task<SalaryDisbursement> DisburseSalariesAsync(SalaryDisbursement salaryDisbursement, int userId, List<int> employeeIds)
        {
            var user = await _bankingDbContext.UserTable
                .Include(u => u.ClientObject)
                .ThenInclude(c => c.SalaryDisbursementList)
                .Include(u => u.ClientObject)
                .ThenInclude(c => c.EmployeeList)
                .FirstOrDefaultAsync(c => c.Id == userId);

            // Check if user or their ClientObject is null
            if (user == null || user.ClientObject == null)
            {
                throw new InvalidOperationException("User or client object not found.");
            }

            var salaryForList = employeeIds
                .Where(eId => user.ClientObject.EmployeeList.Any(e => e.EmployeeId == eId && e.IsActive)) // Ensure employee is active
                .Select(eId => new SalaryFor { EmployeeId = eId })
                .ToList();

            if (salaryForList.Count == 0)
            {
                throw new InvalidOperationException("No valid employees found for salary disbursement.");
            }

            salaryDisbursement.SalaryForList = salaryForList;
            user.ClientObject.SalaryDisbursementList.Add(salaryDisbursement);
            await _bankingDbContext.SaveChangesAsync();

            return salaryDisbursement;
        }

        // Payment method
        public async Task<Payment> CreatePaymentAsync(Payment payment, int beneficiaryId, int userId)
        {
            // Fetch the user and related client and beneficiaries
            var user = await _bankingDbContext.UserTable
                .Include(x => x.ClientObject)
                .ThenInclude(c => c.BeneficiaryList)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.ClientObject == null)
            {
                throw new InvalidOperationException("User or client not found.");
            }

            // Log the number of beneficiaries retrieved
            var beneficiaries = user.ClientObject.BeneficiaryList;
            Console.WriteLine($"Number of beneficiaries found: {beneficiaries.Count}");

            // Log details of each beneficiary
            foreach (var b in beneficiaries)
            {
                Console.WriteLine($"Beneficiary ID: {b.Id}, IsActive: {b.IsActive}");
            }

            // Log the beneficiaryId being searched
            Console.WriteLine($"Searching for Beneficiary with ID: {beneficiaryId}");

            // Find the specific beneficiary
            var beneficiary = beneficiaries
                .FirstOrDefault(b => b.Id == beneficiaryId && b.IsActive);

            if (beneficiary == null)
            {
                throw new InvalidOperationException("Beneficiary not found or is not active.");
            }

            // Create and assign payment
            payment.CreatedAt = DateTime.UtcNow;
            payment.Status = "Pending"; // You can modify this based on your logic

            // Add the payment to the beneficiary's payment list
            beneficiary.PaymentsList ??= new List<Payment>();
            beneficiary.PaymentsList.Add(payment);

            // Save changes
            await _bankingDbContext.SaveChangesAsync();

            return payment;
        }




        // Updated method to get active beneficiaries using the GetBeneficiariesAsync method
        public async Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int userId)
        {
            // Retrieve all beneficiaries for the client
            var beneficiaries = await GetBeneficiariesAsync(userId);

            // Filter and return only active beneficiaries
            return beneficiaries.Where(b => b.IsActive);
        }

        // Get Payment by ID
        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _bankingDbContext.PaymentTable
                .Include(p => p.Transactions) // Include related transactions if necessary
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        // In your ClientService class

        public async Task<ICollection<Beneficiary>> GetRecentPaymentsWithBeneficiaryAsync(int clientId)
        {
           Client client = _bankingDbContext.ClientTable.Include(b=>b.BeneficiaryList).ThenInclude(p=>p.PaymentsList).ThenInclude(t=>t.Transactions)
                .FirstOrDefault(client=>client.Id == clientId);

            return client.BeneficiaryList;

        }
        public async Task<ICollection<Beneficiary>> GetPaginatedRecentPaymentsWithBeneficiaryAsync(int clientId, int pageNumber, int pageSize)
        {
            var client = _bankingDbContext.ClientTable
                .Include(b => b.BeneficiaryList)
                .ThenInclude(p => p.PaymentsList)
                .ThenInclude(t => t.Transactions)
                .FirstOrDefault(client => client.Id == clientId);

            if (client == null || client.BeneficiaryList == null)
            {
                return new List<Beneficiary>(); // Return empty list if no records found
            }

            var paginatedBeneficiaryList = client.BeneficiaryList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return paginatedBeneficiaryList;
        }

        public async Task<(ICollection<Beneficiary>,int totalCount)> GetBeneficiariesForOptionAsync(int userId, int page, int pageSize)
        {
            
            User user = await _bankingDbContext.UserTable.Include(x => x.ClientObject)
               .ThenInclude(y => y.BeneficiaryList)
               .ThenInclude(z => z.AccountDetailsObject)
               .FirstOrDefaultAsync(y => y.Id == userId);

            int count = user.ClientObject.BeneficiaryList.Count();

            var paginatedUser = user.ClientObject.BeneficiaryList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return(paginatedUser, count);

        }


        public async Task<List<SalaryDisbursementResponseDto>> GetSalaryDisbursementsAsync(int userId)
        {
            User user = await _bankingDbContext.UserTable
                .Include(c => c.ClientObject)
                .ThenInclude(s => s.SalaryDisbursementList)
                .ThenInclude(st=>st.SalaryForList)
                .Include(c => c.ClientObject)
                .ThenInclude(s => s.SalaryDisbursementList)
                .ThenInclude(t => t.TransactionList)
                .FirstOrDefaultAsync(u=>u.Id == userId);
            var salaryDisbursementList = user.ClientObject.SalaryDisbursementList;

            List<SalaryDisbursementResponseDto>  salaryDisbursementResponseDtos = new List<SalaryDisbursementResponseDto>();

            foreach(var salarydisbursement in salaryDisbursementList)
            {
                SalaryDisbursementResponseDto responseDto = new SalaryDisbursementResponseDto()
                {
                    Id = salarydisbursement.Id,
                    Amount = salarydisbursement.Amount,
                    ProcessedAt = salarydisbursement.ProcessedAt,
                    Status = salarydisbursement.Status,
                    TransactionList = salarydisbursement.TransactionList,
                    EmployeeList = new List<Employee>()
                };
                    
                foreach(var salaryFor in salarydisbursement.SalaryForList)
                {
                   var employee =  _bankingDbContext.EmployeeTable.Find(salaryFor.EmployeeId);
                    responseDto.EmployeeList.Add(employee);
                }   
                salaryDisbursementResponseDtos.Add(responseDto);
            }
            return salaryDisbursementResponseDtos;
        }
        public async Task<List<SalaryDisbursementResponseDto>> GetPaginatedSalaryDisbursementsAsync(int userId, int pageNumber, int pageSize)
        {
            User user = await _bankingDbContext.UserTable
                .Include(c => c.ClientObject)
                .ThenInclude(s => s.SalaryDisbursementList)
                .ThenInclude(st => st.SalaryForList)
                .Include(c => c.ClientObject)
                .ThenInclude(s => s.SalaryDisbursementList)
                .ThenInclude(t => t.TransactionList)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.ClientObject == null)
            {
                return new List<SalaryDisbursementResponseDto>(); // Return empty list if no records found
            }

            var salaryDisbursementList = user.ClientObject.SalaryDisbursementList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            List<SalaryDisbursementResponseDto> salaryDisbursementResponseDtos = new List<SalaryDisbursementResponseDto>();

            foreach (var salarydisbursement in salaryDisbursementList)
            {
                SalaryDisbursementResponseDto responseDto = new SalaryDisbursementResponseDto()
                {
                    Id = salarydisbursement.Id,
                    Amount = salarydisbursement.Amount,
                    ProcessedAt = salarydisbursement.ProcessedAt,
                    Status = salarydisbursement.Status,
                    TransactionList = salarydisbursement.TransactionList,
                    EmployeeList = new List<Employee>()
                };

                foreach (var salaryFor in salarydisbursement.SalaryForList)
                {
                    var employee = _bankingDbContext.EmployeeTable.Find(salaryFor.EmployeeId);
                    responseDto.EmployeeList.Add(employee);
                }

                salaryDisbursementResponseDtos.Add(responseDto);
            }
            return salaryDisbursementResponseDtos;
        }

        public async Task<ICollection<AuditLog>> GetAuditLogs(int userId)

        {
            // Fetch audit logs that are related to the specific user
            User user = await _bankingDbContext.UserTable.Include(a=>a.AuditLogList).FirstOrDefaultAsync(p=>p.Id == userId);
            return user.AuditLogList;   
        }


    }
}
