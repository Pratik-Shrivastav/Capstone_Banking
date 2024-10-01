﻿using Capstone_Banking.Model;

namespace Capstone_Banking.Service
{
    public interface IClientService
    {
        // Employee methods
        Task<Employee> AddEmployeeAsync(Employee employee,int userId);
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);

        // Beneficiary methods
        Task<Beneficiary> AddBeneficiaryAsync(Beneficiary beneficiary, int userId);
        Task<IEnumerable<Beneficiary>> GetBeneficiariesAsync();
        Task<Beneficiary> GetBeneficiaryByIdAsync(int id);
        Task<Beneficiary> UpdateBeneficiaryAsync(Beneficiary beneficiary);
        Task DeleteBeneficiaryAsync(int id);
    }
}
