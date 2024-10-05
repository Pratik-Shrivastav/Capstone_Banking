using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Banking.Service
{
    public interface IClientService
    {
        // Employee methods
        Task<Employee> AddEmployeeAsync(Employee employee,int userId);
        public Task<ICollection<Employee>> GetAllEmployeesAsync(int userId);
        public Task<(IEnumerable<Employee> employees, int totalCount)> GetEmployeesPagedAsync(int userId, int page, int pageSize);
        Task<Employee> GetEmployeeByIdAsync(int id);
        public  Task<ICollection<Employee>> SearchEmployeesAsync(int userId, string searchTerm);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);



        // Beneficiary methods
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary, int userId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId);
        public Task<(IEnumerable<Beneficiary> beneficiaries, int totalCount)> GetBeneficiaryPagedAsync(int userId, int page, int pageSize);
        public Task<ICollection<Beneficiary>> SearchBeneficiaryAsync(int userId, string searchTerm);
        Task<Beneficiary> GetBeneficiaryByIdAsync(int id);
        Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary);
        Task DeleteBeneficiaryAsync(int id);
        Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int userId);

        public Task<(ICollection<Beneficiary>, int totalCount)> GetBeneficiariesForOptionAsync(int userId,int page, int pageSize);



        //Salary Disbursement Methods
        public Task<SalaryDisbursement> DisburseSalariesAsync(SalaryDisbursement salaryDisbursement, int userId, List<int> employeeIds);
        public Task<List<SalaryDisbursementResponseDto>> GetPaginatedSalaryDisbursementsAsync(int userId, int pageNumber, int pageSize);

        Task<List<SalaryDisbursementResponseDto>> GetSalaryDisbursementsAsync(int userId);


        // Payment Methods
        Task<Payment> CreatePaymentAsync(Payment payment, int beneficiaryId, int userId);
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        public Task<ICollection<Beneficiary>> GetPaginatedRecentPaymentsWithBeneficiaryAsync(int clientId, int pageNumber, int pageSize);
        public Task<ICollection<Beneficiary>> GetRecentPaymentsWithBeneficiaryAsync(int clientId);


        //Audit Log Methods
        public Task<ICollection<AuditLog>> GetAuditLogs(int userId);

    }
}
