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
    }
}
