using BCrypt.Net;
using Capstone_Banking.CommonFunction;
using Capstone_Banking.Dto;
using Capstone_Banking.Model;
using Capstone_Banking.Repository;
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
            newClient.Status = "Success";
            newClient.IsActive = true;

            newAccountDetails.AccountNumber = registerDto.AccountNumber;
            newAccountDetails.Branch = registerDto.Branch;
            newAccountDetails.IFSC = registerDto.IFSC;

            newClient.AccountDetailsObject = newAccountDetails;
            newUser.ClientObject = newClient;

            return await _userAuthRepository.AddUser(newUser);
        }

        public async Task<LoginResponse> Login(LoginData loginData)
        {
            User user = await _userAuthRepository.GetUserByUserName(loginData.UserName);
            LoginResponse loginResponse = new LoginResponse();
            if (user == null)
            {
                loginResponse.Message = "User Not Found";
                loginResponse.Success = false;
                return loginResponse;
            }
            else
            {
                bool pass = BCrypt.Net.BCrypt.EnhancedVerify( loginData.Password, user.Password);
        
                if (pass)
                {
                    loginResponse.Message = "Login Successful";
                    loginResponse.Success = true;
                    loginResponse.Token= GenerateToken.GetToken(user, _configuration);
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

                }
                else
                {
                    loginResponse.Message = "Invalid Credentials";
                    loginResponse.Success = false;
                    loginResponse.Status = user.ClientObject.Status;
                }
                return loginResponse;
            }

        }

        public async Task<User> GetUserById(int id)
        {
           return await _userAuthRepository.GetUserByIdDocumnet(id);
        }
    }
}
