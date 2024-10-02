using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_Banking.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private IUserAuthService _userAuthService;
        private UploadHandler _uploadHandler;
        public UserAuthController(IUserAuthService userAuthService, UploadHandler upload)
        {
            _userAuthService = userAuthService;
            _uploadHandler = upload;
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

        [Authorize(Roles = "Client")]
        [HttpPost("Upload")]
        public async Task<string> Post(IFormFile cin, IFormFile aoa, IFormFile pan)
        {
            ICollection<IFormFile> fileList = new List<IFormFile>();
            BankingDbContext bankingDbContext = new BankingDbContext();
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            fileList.Add(aoa);
            fileList.Add(pan);
            fileList.Add(cin);

            if (fileList != null)
            {
                //_uploadHandler.Upload(int.Parse(id), fileList);
                return await (new UploadHandler(bankingDbContext).Upload(int.Parse(userId),fileList));
            }
            return "Empty";
        }

        [HttpGet]
        public async Task<User> GetUser()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return await _userAuthService.GetUserById(int.Parse(userId));
        }



    }
}
