using Capstone_Banking.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capstone_Banking.CommonFunction
{
    public class GenerateToken
    {
        public static string GetToken(User user, IConfiguration configuration)
        {
            var Claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                new Claim("UserId",user.Id.ToString()),
                new Claim("Display Name",user.Name),
                new Claim("Email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                Claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: signIn
                );
            var Token = new JwtSecurityTokenHandler().WriteToken(token);
            return Token;
        }
    }
}
