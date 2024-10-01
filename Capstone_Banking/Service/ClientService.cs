﻿using Capstone_Banking.Model;
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
        public Task<Employee> AddEmployeeAsync(Employee employee, int userId)
        {
            return _clientRepository.AddEmployeeAsync(employee,userId);
        }

        public Task<IEnumerable<Employee>> GetEmployeesAsync(int userId)
        {
            return _clientRepository.GetEmployeesAsync(userId);
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
    }
}
