using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{

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
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password);
            _bankingDbContext.UserTable.Add(user);
            _bankingDbContext.SaveChanges();
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

        [HttpGet("Download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            BankingDbContext bankingDbContext = new BankingDbContext();
            var fileResult = await (new UploadHandler(bankingDbContext)).DownloadFile(fileName);
            if (fileResult == null)
            {
                return NotFound("File not found");
            }
            return fileResult; 
        }

        [HttpPost("ClientStatus/{clientId}")]
        public void PostClientStatus([FromBody] string value)
        {
            
          
        }
    }
}
