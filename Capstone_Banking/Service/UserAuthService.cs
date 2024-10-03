using BCrypt.Net;
using Capstone_Banking.CommonFunction;
using Capstone_Banking.Data;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Capstone_Banking.Service
{
    public class UserAuthService:IUserAuthService
    {
        private IUserAuthRepository _userAuthRepository;
        private IConfiguration _configuration;

        public UserAuthService(IUserAuthRepository userAuthRepository, IConfiguration configuration)
        {
            _userAuthRepository = userAuthRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterUser(RegisterDto registerDto)
        {
            User newUser = new User();
            Client newClient = new Client();
            AccountDetails newAccountDetails = new AccountDetails();
            AuditLog auditLog = new AuditLog()
            {
                Action="Register",
                Timestamp=DateTime.Now,
                Details=$"{registerDto.Role} Registered"

            };

            newUser.Name= registerDto.FounderName;
            newUser.Email = registerDto.Email;
            newUser.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(registerDto.Password);
            newUser.Role = registerDto.Role;
            newUser.UserName = registerDto.UserName;
            newUser.CreatedOn = DateTime.Now;

            newClient.FounderName = registerDto.FounderName;
            newClient.CompanyName = registerDto.CompanyName;
            newClient.Email = registerDto.Email;
            newClient.Address = registerDto.Address;
            newClient.City = registerDto.City;
            newClient.Region = registerDto.Region;
            newClient.PostalCode = registerDto.PostalCode;
            newClient.Country = registerDto.Country;
            newClient.Phone = registerDto.Phone;
            newClient.Status = "Pending";
            newClient.IsActive = true;

            newAccountDetails.AccountNumber = registerDto.AccountNumber;
            newAccountDetails.Branch = registerDto.Branch;
            newAccountDetails.IFSC = registerDto.IFSC;
            newAccountDetails.AccountBalance = 100000;

            newClient.AccountDetailsObject = newAccountDetails;
            newUser.ClientObject = newClient;

            newUser.AuditLogList = new List<AuditLog>();
            newUser.AuditLogList.Add(auditLog);

            return await _userAuthRepository.AddUser(newUser);
        }

        public async Task<LoginResponse> Login(LoginData loginData)
        {
            using (var dbContext = new BankingDbContext()) // Use a single DbContext instance
            {
                User user = await dbContext.UserTable
                    .Include(u=> u.AuditLogList)
                    .Include(u=> u.ClientObject)
                    .FirstOrDefaultAsync(u => u.UserName == loginData.UserName); // Retrieve user

                LoginResponse loginResponse = new LoginResponse();

                if (user == null)
                {
                    loginResponse.Message = "User Not Found";
                    loginResponse.Success = false;
                    return loginResponse;
                }
                else
                {
                    bool pass = BCrypt.Net.BCrypt.EnhancedVerify(loginData.Password, user.Password);

                    if (pass)
                    {
                        loginResponse.Message = "Login Successful";
                        loginResponse.Success = true;
                        loginResponse.Token = GenerateToken.GetToken(user, _configuration);

                        if (user.ClientObject == null) 
                        {
                            loginResponse.Status = "Success";
                        }
                        else
                        {
                            loginResponse.Status = user.ClientObject.Status;

                        }
                        loginResponse.Role = user.Role;
                        loginResponse.Id = user.Id;

                        // Create and save audit log
                        AuditLog auditLog = new AuditLog()
                        {
                            Action = "Login",
                            Timestamp = DateTime.Now,
                            Details = $"{user.Role}:{user.Name} LogIn"
                        };

                        // Ensure AuditLogList is initialized
                        if (user.AuditLogList == null)
                        {
                            user.AuditLogList = new List<AuditLog>();
                        }

                        // Add audit log to user's list
                        user.AuditLogList.Add(auditLog);

                        // Save changes in the same context
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        loginResponse.Message = "Invalid Credentials";
                        loginResponse.Success = false;
                        loginResponse.Status = user.ClientObject?.Status; // Check if ClientObject is null
                    }
                    return loginResponse;
                }
            }
        }


        public async Task<User> GetUserById(int id)
        {
           return await _userAuthRepository.GetUserByIdDocumnet(id);
        }
        public async Task<bool> GetUniqueUsernames(string username)
        {
            // Get the list of users asynchronously
            var users = await _userAuthRepository.GetAllUser();

            // Check if the username exists in the list of usernames
            var isUsernameTaken = users.Any(user => user.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));

            // Return true if username is not taken (unique), false otherwise
            return !isUsernameTaken;
        }



    }
}
