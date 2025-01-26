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
        public async Task<LoginDTO> LoginAsync(LoginRB content)
        {
            var res = new LoginDTO();
            try
            {
                var user = _db.Users.FirstOrDefault(p => p.UserName == content.UserName) ?? throw new Exception("not-found");
                if (user.Password != EncryptPassword(content.Password, content.UserName)) throw new Exception("diff-pass");
                res.Token = GenerateToken(user);
                res.LoggedIn = true;
            }
            catch (Exception ex)
            {
                res.LoggedIn = false;
                res.Message = ex.Message;
            }
            return res;
        }
        public async Task<RegisterDTO> RegisterAsync(RegisterRB content)
        {
            var res = new RegisterDTO();
            try
            {
                if (_db.Users.Any(p => p.UserName == content.UserName)) throw new Exception("key-found");
                var user = new AppUser
                {
                    UserName = content.UserName,
                    Email = content.Email,
                    Password = EncryptPassword(content.Password, content.UserName),
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
        public async Task<ResetPasswordDTO> ResetPasswordAsync(ResetPasswordRB content)
        {
            var res = new ResetPasswordDTO();
            try
            {
                var user = _db.Users.FirstOrDefault(p => p.UserName == content.UserName) ?? throw new Exception("not-found");
                user.Password = EncryptPassword(content.Password, content.UserName);
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

        private string EncryptPassword(string password, string username)
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
        private string GenerateToken(AppUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
