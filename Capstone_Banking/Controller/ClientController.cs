using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{
    [Authorize(Roles = "Client")]

    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly BankingDbContext _bankingDbContext;

        public ClientController(IClientService clientService, BankingDbContext bankingDbContext)
        {
            _clientService = clientService;
            _bankingDbContext = bankingDbContext;
        }


        // POST: api/Client/Employee
        [HttpPost("Employee")]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (employee == null)
            {
                return BadRequest("Employee is null");
            }

            try
            {
                employee.CreatedAt = DateTime.UtcNow; // Set createdAt date
                await _clientService.AddEmployeeAsync(employee, int.Parse(userId)); // Add employee
                AddAuditLogs.AddLog(int.Parse(userId), "Employee Add", $" Added Employee");
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}"); // Log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // POST: api/Client/Beneficiary
        [HttpPost("Beneficiary")]
        public async Task<IActionResult> PostBeneficiary(Beneficiary beneficiary)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (beneficiary == null)
            {
                return BadRequest("Beneficiary is null");
            }

            var createdBeneficiary = await _clientService.AddBeneficiaryAsync(beneficiary, int.Parse(userId));
            AddAuditLogs.AddLog(int.Parse(userId), "Beneficiary Add", $" Added Beneficiary");
            return CreatedAtAction(nameof(GetBeneficiaryById), new { id = createdBeneficiary.Id }, createdBeneficiary);
        }

        // GET: api/Client/Employees
        [HttpGet("Employees")]
        public async Task<IActionResult> GetEmployees()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var employees = await _clientService.GetEmployeesAsync(int.Parse(userId));
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
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            var beneficiaries = await _clientService.GetBeneficiariesAsync(int.Parse(userId));
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
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            employee.EmployeeId = id;
            var updatedEmployee = await _clientService.UpdateEmployeeAsync(employee);
            if (updatedEmployee == null)
            {
                return NotFound("Employee not found");
            }
            AddAuditLogs.AddLog(int.Parse(userId), "Employee Updated", $" Updated EmployeeId: {id}");


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
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (updatedBeneficiary == null)
            {
                return NotFound("Beneficiary not found");
            }
            AddAuditLogs.AddLog(int.Parse(userId), "Beneficiary Updated", $" Updated BeneficiaryId: {id}");

            return Ok(updatedBeneficiary);
        }

        // DELETE: api/Client/Employee/{id}
        [HttpDelete("Employee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "Employee Deleted", $" Deleted EmployeeId: {id}");

            await _clientService.DeleteEmployeeAsync(id);
            return NoContent();
        }

        // DELETE: api/Client/Beneficiary/{id}
        [HttpDelete("Beneficiary/{id}")]
        public async Task<IActionResult> DeleteBeneficiary(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "Beneficiary Deleted", $" Deleted BeneficiaryId: {id}");

            await _clientService.DeleteBeneficiaryAsync(id);
            return NoContent();
        }
        // POST: api/Client/disburse-salaries
        [HttpPost("disburse-salaries")]
        public async Task<IActionResult> DisburseSalaries([FromBody] SalaryDisbursementRequest request)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (request == null || request.EmployeeIds == null || request.EmployeeIds.Count == 0)
            {
                return BadRequest("Invalid request. Please provide employee IDs.");
            }

            var salaryDisbursement = new SalaryDisbursement
            {
                Amount = request.Amount,
                Status = "Pending",  // You can set the status as needed
                ProcessedAt = DateTime.UtcNow
            };

            // Call the service to disburse salaries
            var result = await _clientService.DisburseSalariesAsync(salaryDisbursement, int.Parse(userId), request.EmployeeIds);
            AddAuditLogs.AddLog(int.Parse(userId), "Salary Disbursement", $" created Salary Disbursement: {result.Id}");
            return Ok(result); // Return the disbursement information
        }
        public class SalaryDisbursementRequest
        {
            public double Amount { get; set; }
            public List<int> EmployeeIds { get; set; } = new List<int>();
        }

       
        // POST: api/Client/Beneficiary/Payment
        [HttpPost("Beneficiary/Payment")]
        public async Task<IActionResult> MakePayment([FromBody] BeneficiaryPaymentRequest request)
        {
            Console.WriteLine($"BeneficiaryId: {request.BeneficiaryId}, PaymentType: {request.PaymentType}, Amount: {request.Amount}");
            // Retrieve UserId from claims in headers
            string userIdString = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized("User ID is missing in the request.");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Invalid User ID format.");
            }

            // Validate the request details
            if (request == null || request.Amount <= 0 || string.IsNullOrEmpty(request.PaymentType))
            {
                return BadRequest("Invalid payment data.");
            }

            try
            {
                // Create a new Payment object and set default values
                var payment = new Payment
                {
                    PaymentType = request.PaymentType,
                    Amount = request.Amount,
                    Status = "Pending", // Default status is Pending
                    CreatedAt = DateTime.UtcNow,
                    ApprovedBy = 0,     // Will remain null until approved
                    ApprovedAt = null   // Will remain null until approved
                };

                // Call service method to create the payment
                var createdPayment = await _clientService.CreatePaymentAsync(payment, request.BeneficiaryId, userId);
                AddAuditLogs.AddLog(int.Parse(userIdString), "Payment", $" created Payment: {createdPayment.Id}");

                return Ok(createdPayment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // GET: api/Client/Payment/{id}
        [HttpGet("Payment/{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _clientService.GetPaymentByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        // Define the request model for Beneficiary Payment
        public class BeneficiaryPaymentRequest
        {
            public int BeneficiaryId { get; set; }
            public string PaymentType { get; set; }
            public double Amount { get; set; }
        }


        // GET: api/Client/Payments/Recent
        [HttpGet("Payments/Recent")]
        public async Task<IActionResult> GetRecentPayments()
        {
            string userIdString = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            var payments = await _clientService.GetRecentPaymentsWithBeneficiaryAsync(int.Parse(userIdString));
            return Ok(payments);
        }

        // GET: api/Client/Payments/SalaryDisbursements
        [HttpGet("Payments/SalaryDisbursements")]
        public async Task<IActionResult> GetSalaryDisbursements()
        {
            var salaryDisbursements = await _clientService.GetSalaryDisbursementsAsync();
            return Ok(salaryDisbursements);
        }

    }
    }

