﻿using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        
        // POST: api/Client/Employee
        [HttpPost("Employee")]
        public async Task<IActionResult> PostEmployee(Employee employee)
        {
            if (employee == null)
            {
                return BadRequest("Employee is null");
            }

            var createdEmployee = await _clientService.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.EmployeeId }, createdEmployee);
        }

        // POST: api/Client/Beneficiary
        [HttpPost("Beneficiary")]
        public async Task<IActionResult> PostBeneficiary(Beneficiary beneficiary)
        {
            if (beneficiary == null)
            {
                return BadRequest("Beneficiary is null");
            }

            var createdBeneficiary = await _clientService.AddBeneficiaryAsync(beneficiary);
            return CreatedAtAction(nameof(GetBeneficiaryById), new { id = createdBeneficiary.Id }, createdBeneficiary);
        }

        // GET: api/Client/Employees
        [HttpGet("Employees")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = await _clientService.GetEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/Client/Employee/{id}
        [HttpGet("Employee/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await _clientService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // GET: api/Client/Beneficiaries
        [HttpGet("Beneficiaries")]
        public async Task<IActionResult> GetBeneficiaries()
        {
            var beneficiaries = await _clientService.GetBeneficiariesAsync();
            return Ok(beneficiaries);
        }

        // GET: api/Client/Beneficiary/{id}
        [HttpGet("Beneficiary/{id}")]
        public async Task<IActionResult> GetBeneficiaryById(int id)
        {

            var beneficiary = await _clientService.GetBeneficiaryByIdAsync(id);
            if (beneficiary == null)
            {
                return NotFound();
            }

            return Ok(beneficiary);
        }

        // PUT: api/Client/Employee/{id}
        [HttpPut("Employee/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {/*
            if (id != employee.EmployeeId)
            {
                employee.EmployeeId = id;
                return BadRequest("Employee ID mismatch");
            }*/
            employee.EmployeeId = id;
            var updatedEmployee = await _clientService.UpdateEmployeeAsync(employee);
            if (updatedEmployee == null)
            {
                return NotFound("Employee not found");
            }

            return Ok(updatedEmployee);
        }

        // PUT: api/Client/Beneficiary/{id}
        [HttpPut("Beneficiary/{id}")]
        public async Task<IActionResult> UpdateBeneficiary(int id, Beneficiary beneficiary)
        {
          /*  if (id != beneficiary.Id)
            {
                return BadRequest("Beneficiary ID mismatch");
            }*/
          beneficiary.Id = id;

            var updatedBeneficiary = await _clientService.UpdateBeneficiaryAsync(beneficiary);
            if (updatedBeneficiary == null)
            {
                return NotFound("Beneficiary not found");
            }

            return Ok(updatedBeneficiary);
        }

        // DELETE: api/Client/Employee/{id}
        [HttpDelete("Employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _clientService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        // DELETE: api/Client/Beneficiary/{id}
        [HttpDelete("Beneficiary/{id}")]
        public async Task<IActionResult> DeleteBeneficiary(int id)
        {
            await _clientService.DeleteBeneficiaryAsync(id);
            return NoContent();
        }
    }
}
