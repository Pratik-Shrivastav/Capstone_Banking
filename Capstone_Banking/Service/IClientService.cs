using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Banking.Service
{
    public interface IClientService
    {
        // Employee methods
        Task<Employee> AddEmployeeAsync(Employee employee,int userId);
        public Task<(IEnumerable<Employee> employees, int totalCount)> GetEmployeesAsync(int userId, int page, int pageSize);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);

        // Beneficiary methods
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary, int userId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId);
        Task<Beneficiary> GetBeneficiaryByIdAsync(int id);
        Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary);
        Task DeleteBeneficiaryAsync(int id);

       public Task<SalaryDisbursement> DisburseSalariesAsync(SalaryDisbursement salaryDisbursement, int userId, List<int> employeeIds);
        Task<Payment> CreatePaymentAsync(Payment payment, int beneficiaryId, int userId);
        Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int userId);
        Task<Payment> GetPaymentByIdAsync(int paymentId);

        public Task<ICollection<Beneficiary>> GetRecentPaymentsWithBeneficiaryAsync(int clientId);
        Task<List<SalaryDisbursementResponseDto>> GetSalaryDisbursementsAsync(int userId);

        public Task<ICollection<AuditLog>> GetAuditLogs(int userId);

    }
}
