using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{
    [Authorize(Roles = "SuperAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuperAdminController : ControllerBase
    {
        private ISuperAdminService _superAdminService;
        private BankingDbContext _bankingDbContext;
        public SuperAdminController(ISuperAdminService superAdminService, BankingDbContext bankingDbContext)
        {
            _superAdminService = superAdminService;
            _bankingDbContext = bankingDbContext;
        }

        // GET: api/<SuperAdminController>
        [HttpGet]
        public Task<ICollection<Client>> Get()
        {
            return _superAdminService.GetAllClients();
        }

        [HttpPost]
        public User Post([FromBody] User user)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
            _bankingDbContext.UserTable.Add(user);
            _bankingDbContext.SaveChanges();
            AddAuditLogs.AddLog(int.Parse(userId),"Super Admin Registered", "Registered");
           return user;
        }

        [HttpGet("{clientId}")]
        public async Task<Client> GetById(int clientId)
        {
            
            return await _superAdminService.GetClientsById(clientId);
        }

        [HttpGet("Document/{id}")]
        public async Task<ICollection<Documents>> GetDocumentsNameOfClient(int id)
        {

            return await _superAdminService.GetDocumentById(id);
        }

        [HttpGet("SalaryDisbursement/{clientId}")]
        public async Task<ICollection<SalaryDisbursementResponseDto>> GetSalaryDisbursementClient(int clientId)
        {

            return await _superAdminService.GetSalaryDisbursementClient(clientId);
        }

        [HttpGet("Download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            
            BankingDbContext bankingDbContext = new BankingDbContext();
            var fileResult = await (new UploadHandler(bankingDbContext)).DownloadFile(fileName);
            if (fileResult == null)
            {
                return NotFound("File not found");
            }
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "File Download", "File Downloaded");

            return fileResult; 
        }

        [HttpPut("ClientStatus/{clientId}")]
        public void ClientStatus(int clientId,[FromBody] string value)
        {
            _superAdminService.UpdateClientStatus(clientId, value);
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "Client Status", value);


        }

        [HttpPut("PaymentStatus/{clientId}/{paymentId}")]
        public void PaymentStatus(int clientId,int paymentId, [FromBody] string value)
        {
            Console.WriteLine(clientId);
            _superAdminService.UpdatePaymentStatus(clientId,paymentId, value);
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "Payment Status", $"ClientId: {clientId} PaymentId-{paymentId} {value}");
        }

        [HttpPut("SalaryDisbursementStatus/{clientId}/{salaryDisId}")]
        public void SalaryDisbursementStatus(int clientId, int salaryDisId, [FromBody] string value)
        {
            Console.WriteLine(clientId);
            _superAdminService.UpdateSalaryDisbursementStatus(clientId,salaryDisId, value);
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "Salary Disbursement Status", $"ClientId: {clientId} SalaryId-{salaryDisId} {value}");
        }
    }
}
