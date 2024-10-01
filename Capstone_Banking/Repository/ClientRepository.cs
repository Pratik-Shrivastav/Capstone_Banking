﻿using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Microsoft.EntityFrameworkCore;


namespace Capstone_Banking.Repository
{
    public class ClientRepository : IClientRepository
    {
        private BankingDbContext _bankingDbContext;
        public ClientRepository(BankingDbContext bankingDbContext)
        {
            _bankingDbContext = bankingDbContext;
        }
        public async Task<Employee> AddEmployeeAsync(Employee employee, int userId)
        {
            // Fetch the client to associate the employee with
            

            var user = await _bankingDbContext.UserTable.Include(c => c.ClientObject).ThenInclude(p=>p.EmployeeList)
                .FirstOrDefaultAsync(c=>c.Id == userId);
            employee.CreatedAt = DateTime.UtcNow;
            user.ClientObject.EmployeeList.Add(employee);
            await _bankingDbContext.SaveChangesAsync();

            return employee;
        }


        // Create Beneficiary
        public async Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary,int userId)
        {
            var user = await _bankingDbContext.UserTable.Include(c => c.ClientObject).ThenInclude(p => p.BeneficiaryList)
                .FirstOrDefaultAsync(c => c.Id == userId);
            user.ClientObject.BeneficiaryList.Add(beneficiary);
            await _bankingDbContext.SaveChangesAsync();
            return beneficiary;
        }

        // Get All Employees
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(int userId)
        {
            User user = await _bankingDbContext.UserTable.Include(x=>x.ClientObject)
                .ThenInclude(y=>y.EmployeeList)
                .ThenInclude(z=>z.AccountDetailsObject).FirstOrDefaultAsync(y=>y.Id == userId);
            return user.ClientObject.EmployeeList;
        }

        // Get All Beneficiaries
        public async Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId)
        {
            User user= await _bankingDbContext.UserTable.Include(x=>x.ClientObject)
                .ThenInclude(y=>y.BeneficiaryList).ThenInclude(z=>z.AccountDetailsObject)
                .FirstOrDefaultAsync(y => y.Id == userId);
            return user.ClientObject.BeneficiaryList;
        }

        // Get Employee by ID with related entities
        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _bankingDbContext.EmployeeTable
                .Include(e => e.AccountDetailsObject)  // Include Account Details
                      
                .FirstOrDefaultAsync(e => e.EmployeeId == id); // Use FirstOrDefaultAsync for better handling of nulls
        }


        // Get Beneficiary by ID with related entities
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
                employee.isActive = false;
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
                beneficiary.IsActive = false;
                _bankingDbContext.BeneficiaryTable.Update(beneficiary);
                await _bankingDbContext.SaveChangesAsync();
            }
        }

    }
}
