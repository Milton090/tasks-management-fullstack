using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Models;

namespace TaskManagement.Helpers
{
    public class AuthHelper
    {
        private readonly IPasswordHasher<String> _passwordHasher;
        private readonly IConfiguration _config;
        public AuthHelper(IConfiguration config)
        {
            _passwordHasher = new PasswordHasher<String>();
            _config = config;
        }

        public String HashPassword(String email, String password)
        {
            return _passwordHasher.HashPassword(email, password);
        }

        public Boolean VerifyPassword(String email, String password, String hashedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(email, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }


        public String GenerateToken(Int32 userId)
        {
            Claim[] claims = new Claim[]
            {
                new("userId", userId.ToString())
            };

            SymmetricSecurityKey key = new(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:JWT_SECRET").Value));

            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256Signature);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler handler = new();

            SecurityToken token = handler.CreateToken(descriptor);

            return handler.WriteToken(token);
        }
    }
}
