using Capstone_Banking.Dto;
using Capstone_Banking.Model;

namespace Capstone_Banking.Service
{
    public interface IUserAuthService
    {
        public Task<User> RegisterUser(RegisterDto registerDto);
        public Task<LoginResponse> Login(LoginData loginData);

        public Task<User> GetUserById(int id);


    }
}
