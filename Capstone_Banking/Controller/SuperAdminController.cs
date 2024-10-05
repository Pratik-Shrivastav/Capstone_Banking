using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private UploadHandler _uploadHandler;

        public SuperAdminController(ISuperAdminService superAdminService, BankingDbContext bankingDbContext, UploadHandler uploadHandler)
        {
            _superAdminService = superAdminService;
            _bankingDbContext = bankingDbContext;
            _uploadHandler = uploadHandler;
        }

        [HttpGet("AllClients")]
        public async Task<IActionResult> Get()
        {
            return Ok(_superAdminService.GetAllClients());
        }
        // GET: api/<SuperAdminController>
        [HttpGet("SuccessClients")]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 8)
        {
            var (clients, totalCount) = await _superAdminService.GetAllClientsPaged(page, pageSize);
            return Ok(new { clients, totalCount });
        }

        [HttpGet("PendingClients")]
        public async Task<IActionResult> GetPagedPending(int page = 1, int pageSize = 8)
        {
            var (clients, totalCount) = await _superAdminService.GetAllClientsPagedPending(page, pageSize);
            return Ok(new { clients, totalCount });
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

        [HttpGet("ClientByName/{companyName}/{status}")]
        public async Task<ICollection<User>> GetByName(string companyName,string status)
        {
            return await _superAdminService.GetClientName(companyName,status);

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
            string fileResult = await _uploadHandler.DownloadFile(fileName);
            if (fileResult.Length == 0)
            {
                return NotFound("File not found");
            }
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "File Download", "File Downloaded");

            return Ok(new { documentUrl = fileResult }); ;
        }

        [HttpPut("ClientStatus/{clientId}")]
        public void ClientStatus(int clientId,[FromBody] string value)
        {
            _superAdminService.UpdateClientStatus(clientId, value);
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            AddAuditLogs.AddLog(int.Parse(userId), "Client Status", value);


        }

        [HttpPut("PaymentStatus/{clientId}/{beneficiaryId}/{paymentId}")]
        public void PaymentStatus(int clientId,int beneficiaryId, int paymentId, [FromBody] string value)
        {
            Console.WriteLine(clientId);
            _superAdminService.UpdatePaymentStatus(clientId, beneficiaryId, paymentId, value);
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
