using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private IUserAuthService _userAuthService;
        public UserAuthController(IUserAuthService userAuthService)
        {
            _userAuthService = userAuthService;
        }
        

        // POST api/<UserAuthController>
        [HttpPost("Register")]
        public async Task<User> Post([FromBody] RegisterDto registerDto)
        {
            return await (_userAuthService.RegisterUser(registerDto));
        }

        [HttpPost("Login")]
        public async Task<LoginResponse> Login([FromBody] LoginData loginData)
        {
            return await _userAuthService.Login(loginData);
        }

        [HttpPost("Upload")]
        public void Post(ICollection<IFormFile> fileList)
        {
            BankingDbContext bankingDbContext = new BankingDbContext();
            string id = "2";

            if (fileList != null)
            {
                (new UploadHandler(bankingDbContext)).Upload(int.Parse(id), fileList);
            }
        }


    }
}
