﻿using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Repository
{
    public interface IClientRepository
    {
        Task<Employee> AddEmployeeAsync(Employee employee,int userId);
        Task<IEnumerable<Employee>> GetEmployeesAsync(int userId);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);

        // Beneficiary methods
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary,int userId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId);
        Task<Beneficiary> GetBeneficiaryByIdAsync(int id);
        Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary);
        Task DeleteBeneficiaryAsync(int id);

        Task<SalaryDisbursement> DisburseSalariesAsync(SalaryDisbursement salaryDisbursement, int userId, List<int> employeeIds);
        Task<Payment> CreatePaymentAsync(Payment payment, int beneficiaryId, int userId);
        Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int userId);
        Task<Payment> GetPaymentByIdAsync(int paymentId);

        public Task<List<PaymentWithBeneficiaryDto>> GetRecentPaymentsWithBeneficiaryAsync();
        public  Task<List<SalaryDisbursementResponseDto>> GetSalaryDisbursementsAsync();
    }
}
