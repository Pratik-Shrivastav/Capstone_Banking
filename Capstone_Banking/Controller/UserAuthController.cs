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
        private BankingDbContext _bankingDbContext;
        public UserAuthController(IUserAuthService userAuthService, UploadHandler upload,BankingDbContext bankingDbContext)
        {
            _userAuthService = userAuthService;
            _uploadHandler = upload;
            _bankingDbContext = bankingDbContext;
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
                //return await (new UploadHandler(bankingDbContext).Upload(int.Parse(userId),fileList));
                return await _uploadHandler.Upload(int.Parse(userId), fileList);

            }
            return "Empty";
        }

        [Authorize(Roles = "Client")]
        [HttpGet]
        public async Task<User> GetUser()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            return await _userAuthService.GetUserById(int.Parse(userId));
        }

        [HttpGet("Usernames/{username}")]
        public async Task<bool> GetUsernames(string username)
        {
            var usernamesPresent = await _userAuthService.GetUniqueUsernames(username);
            return usernamesPresent;
        }

        [HttpGet("AccountNumber/{accountNumber}")]
        public async Task<bool> GetClientsAccountNumber(string accountNumber)
        {
            var accountNumberPresent = await _userAuthService.GetUniqueAccountNumbers(accountNumber);
            return accountNumberPresent;
        }

        [Authorize(Roles = "Client")]

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
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            double otp = GenerateOtp.GenerateOtpToEmail();
            User user = _bankingDbContext.UserTable.FirstOrDefault(f=>f.UserName == forgotPasswordDto.Username);
            user.OTP = otp;
            user.isVerified = false;
            _bankingDbContext.SaveChanges();

            EmailHandler.SendEmail(user.Email, "OTP to reset Password", $"{otp}");

            return StatusCode(200);
        }

        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto verifyOtpDto)
        {

            User user = _bankingDbContext.UserTable.FirstOrDefault(t=>t.UserName == verifyOtpDto.Username);
            if (user.OTP == double.Parse(verifyOtpDto.Otp)) 
            {
                user.isVerified = true;
                _bankingDbContext.SaveChanges();
                return StatusCode(200);

            }
            // Check if OTP matches for the email
            // Return success or failure response
           return BadRequest("Otp did not match"); 
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
           
            User user = _bankingDbContext.UserTable.FirstOrDefault(r=>r.UserName == resetPasswordDto.Username);
            if(user.isVerified == false || user.isVerified == null)
            {
                return BadRequest("OTP is not verified. Cannot Reset Password");
            }
            user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(resetPasswordDto.NewPassword);
            _bankingDbContext.SaveChanges();
            return StatusCode(200);
        }


    }
}
