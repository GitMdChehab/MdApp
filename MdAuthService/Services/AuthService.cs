using MdAuthService.Data;
using MdAuthService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using MdAuthService.Interfaces;

namespace MdAuthService.Services
{
    public class AuthService(AuthContext _db, IConfiguration _config) : IAuthService
    {
        public async Task<LoginDTO> LoginAsync(string username, string password)
        {
            var res = new LoginDTO();
            try
            {
                var user = _db.Users.FirstOrDefault(p => p.UserName == username) ?? throw new Exception("not-found");
                if (user.Password != ComputeHmacSha256(password, username)) throw new Exception("diff-pass");
                res.Token = GenerateToken(user);
                res.LoggedIn = true;
            }
            catch (Exception ex)
            {
                res.LoggedIn = false;
                res.Message = ex.Message;
            }
            return await Task.FromResult(res);
        }
        public async Task<RegisterDTO> RegisterAsync(RegisterRequestBody content)
        {
            var res = new RegisterDTO();
            try
            {
                if (_db.Users.Any(p => p.UserName == content.UserName)) throw new Exception("username-found");
                var user = new AppUser
                {
                    UserName = content.UserName,
                    Email = content.Email,
                    Password = ComputeHmacSha256(content.Password, content.UserName),
                    Role = content.Role
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
            }
            return res;
        }
        public string ComputeHmacSha256(string password, string username)
        {
            username = username.ToLower();
            byte[] keyBytes = Encoding.UTF8.GetBytes(username);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public string GenerateToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config["Jwt:Issuer"],
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
