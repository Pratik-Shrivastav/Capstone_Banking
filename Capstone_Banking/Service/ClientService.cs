using Capstone_Banking.Model;
using Capstone_Banking.Repository;

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
        public Task<Employee> AddEmployeeAsync(Employee employee)
        {
            return _clientRepository.AddEmployeeAsync(employee);
        }

        public Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return _clientRepository.GetEmployeesAsync();
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
        public Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary)
        {
            return _clientRepository.AddBeneficiaryAsync(beneficiary);
        }

        public Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync()
        {
            return _clientRepository.GetBeneficiariesAsync();
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
    }
}
