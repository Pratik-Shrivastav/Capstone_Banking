using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_Banking.Service
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        // Employee methods
        public Task<Employee> AddEmployeeAsync(Employee employee, int userId)
        {
            return _clientRepository.AddEmployeeAsync(employee,userId);
        }
        public Task<ICollection<Employee>> GetAllEmployeesAsync(int userId)
        {
            return _clientRepository.GetAllEmployeesAsync(userId);
        }
        public Task<ICollection<Employee>> SearchEmployeesAsync(int userId, string searchTerm)
        {
            return  _clientRepository.SearchEmployeesAsync(userId, searchTerm);
        }


        public Task<(IEnumerable<Employee> employees, int totalCount)> GetEmployeesPagedAsync(int userId, int page, int pageSize)
        {
            return _clientRepository.GetEmployeesPagedAsync(userId, page, pageSize);
        }


        public Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return _clientRepository.GetEmployeeByIdAsync(id);
        }

        public Task<Employee> UpdateEmployeeAsync(Employee employee)
        {
            return _clientRepository.UpdateEmployeeAsync(employee);
        }

        public Task DeleteEmployeeAsync(int id)
        {
            return _clientRepository.DeleteEmployeeAsync(id);
        }

        // Beneficiary methods
        public Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary,int userId)
        {
            return _clientRepository.AddBeneficiaryAsync(beneficiary,userId);
        }

        public Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync(int userId)
        {
            return _clientRepository.GetBeneficiariesAsync(userId);
        }

        public Task<Beneficiary> GetBeneficiaryByIdAsync(int id)
        {
            return _clientRepository.GetBeneficiaryByIdAsync(id);
        }

        public Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary)
        {
            return _clientRepository.UpdateBeneficiaryAsync(beneficiary);
        }

        public Task DeleteBeneficiaryAsync(int id)
        {

            return _clientRepository.DeleteBeneficiaryAsync(id);
        }
       public Task<SalaryDisbursement> DisburseSalariesAsync(SalaryDisbursement salaryDisbursement, int userId, List<int> employeeIds)
        {
            return _clientRepository.DisburseSalariesAsync(salaryDisbursement, userId, employeeIds);
        }
        public async Task<Payment> CreatePaymentAsync(Payment payment, int beneficiaryId, int userId)
        {
            return await _clientRepository.CreatePaymentAsync(payment, beneficiaryId, userId);
        }


        public async Task<IEnumerable<Beneficiary>> GetActiveBeneficiariesAsync(int userId)
        {
            return await _clientRepository.GetActiveBeneficiariesAsync(userId);
        }

        public async Task<(IEnumerable<Beneficiary> beneficiaries, int totalCount)> GetBeneficiaryPagedAsync(int userId, int page, int pageSize)
        {
            return await _clientRepository.GetBeneficiaryPagedAsync(userId, page, pageSize);
        }

        public async Task<ICollection<Beneficiary>> SearchBeneficiaryAsync(int userId, string searchTerm)
        {
            return await _clientRepository.SearchBeneficiaryAsync(userId, searchTerm);
        }


        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _clientRepository.GetPaymentByIdAsync(paymentId);
        }
        public async Task<ICollection<Beneficiary>> GetRecentPaymentsWithBeneficiaryAsync(int clientId)
        {
            return await _clientRepository.GetRecentPaymentsWithBeneficiaryAsync(clientId);
        }
        public async Task<(List<SalaryDisbursementResponseDto>, int totalCount)> GetPaginatedSalaryDisbursementsAsync(int userId, int pageNumber, int pageSize)
        {
            return await _clientRepository.GetPaginatedSalaryDisbursementsAsync(userId, pageNumber, pageSize);
        }

        public async Task<ICollection<Beneficiary>> GetPaginatedRecentPaymentsWithBeneficiaryAsync(int clientId, int pageNumber, int pageSize)
        {
            return await _clientRepository.GetPaginatedRecentPaymentsWithBeneficiaryAsync(clientId, pageNumber, pageSize);
        }
        public async Task<(ICollection<Beneficiary>, int totalCount)> GetBeneficiariesForOptionAsync(int userId,int page, int pageSize)
        {
            return await _clientRepository.GetBeneficiariesForOptionAsync(userId, page, pageSize);

        }

        // New methods for salary disbursement retrieval
        public async Task<List<SalaryDisbursementResponseDto>> GetSalaryDisbursementsAsync(int userId)
        {
            return await _clientRepository.GetSalaryDisbursementsAsync(userId); // Call the repository method for salary disbursements
        }

        public async Task<(ICollection<Payment>, int totalCount)> GetPaymentsForBeneficiaryPaginated(int userId, int beneficiaryId, int pageNumber, int pageSize)
        {
            return await _clientRepository.GetPaymentsForBeneficiaryPaginated(userId, beneficiaryId, pageNumber, pageSize);
        }

        public Task<ICollection<AuditLog>> GetAuditLogs(int userId)

        {
            return _clientRepository.GetAuditLogs(userId);
        }
    }
}
