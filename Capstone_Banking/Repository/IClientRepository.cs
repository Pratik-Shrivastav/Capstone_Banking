﻿using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Banking.Repository
{
    public interface IClientRepository
    {
        Task<Employee> AddEmployeeAsync(Employee employee,int userId);

        public Task<ICollection<Employee>> GetAllEmployeesAsync(int userId);
        public Task<(IEnumerable<Employee> employees, int totalCount)> GetEmployeesPagedAsync(int userId, int page, int pageSize);
        Task<Employee> GetEmployeeByIdAsync(int id);
        public  Task<ICollection<Employee>> SearchEmployeesAsync(int userId, string searchTerm);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);

        // Beneficiary methods
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary,int userId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId);
        public Task<(IEnumerable<Beneficiary> beneficiaries, int totalCount)> GetBeneficiaryPagedAsync(int userId, int page, int pageSize);
        public Task<ICollection<Beneficiary>> SearchBeneficiaryAsync(int userId, string searchTerm);


        Task<Beneficiary> GetBeneficiaryByIdAsync(int id);
        Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary);
        Task DeleteBeneficiaryAsync(int id);

        Task<SalaryDisbursement> DisburseSalariesAsync(SalaryDisbursement salaryDisbursement, int userId, List<int> employeeIds);
        public Task<(List<SalaryDisbursementResponseDto>, int totalCount)> GetPaginatedSalaryDisbursementsAsync(int userId, int pageNumber, int pageSize);

        Task<Payment> CreatePaymentAsync(Payment payment, int beneficiaryId, int userId);
        public  Task<ICollection<Beneficiary>> GetPaginatedRecentPaymentsWithBeneficiaryAsync(int clientId, int pageNumber, int pageSize);

        public Task<(ICollection<Beneficiary>, int totalCount)> GetBeneficiariesForOptionAsync(int userId,int page, int pageSize);

        Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int userId);
        Task<Payment> GetPaymentByIdAsync(int paymentId);

        public Task<ICollection<Beneficiary>> GetRecentPaymentsWithBeneficiaryAsync(int clientId);
        public  Task<List<SalaryDisbursementResponseDto>> GetSalaryDisbursementsAsync(int userId);

        public Task<(ICollection<Payment>, int totalCount)> GetPaymentsForBeneficiaryPaginated(int userId, int beneficiaryId,int pageNumber, int pageSize);

        public Task<(ICollection<Beneficiary>, int totalCount)> SearchBeneficiariesAsync(int userId, string searchTerm, int pageNumber, int pageSize);
        public Task<(ICollection<Payment>, int totalCount)> SearchPaymentsForBeneficiaryAsync(int userId, int beneficiaryId, string searchTerm, int pageNumber, int pageSize);

        public Task<ICollection<AuditLog>> GetAuditLogs(int userId);

        public (ICollection<Client>, int count) GetInboundClients(int page, int pageSize);
        public (ICollection<Client>, int count) GetSearchedInboundClients(string searchTerm, int page, int pageSize);

        public void AddInboundBeneficiary(int userId, int clientToBeAdded);


    }
}
