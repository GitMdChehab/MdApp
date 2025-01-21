using MdAuthService.Data;
using MdAuthService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MdAuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration _config, AuthContext db) : ControllerBase
    {
        [HttpPost("login")]
        public Task<LoginDTO> Login(string username, string password)
        {
            LoginDTO res = new LoginDTO();
            try
            {
                var user = db.Users.FirstOrDefault(p => p.UserName == username) ?? throw new Exception("not-found");
                if (user.Password != ComputeHmacSha256(password, username)) throw new Exception("diff-pass");
                res.Token = GenerateToken(user);
                res.LoggedIn = true;
            }
            catch (Exception ex)
            {
                res.LoggedIn = false;
            }
            return Task.FromResult(res);
        }
        [HttpPost("register")]
        public Task<RegisterDTO> Register(RegisterRequestBody content)
        {
            RegisterDTO res = new RegisterDTO();
            try
            {
                if (db.Users.Any(p => p.UserName == content.UserName)) throw new Exception("username-found");
                var user = new AppUser()
                {
                    UserName = content.UserName,
                    Email = content.Email,
                    Password = ComputeHmacSha256(content.Password, content.UserName),
                    Role = content.Role
                };
                db.Users.Add(user);
                db.SaveChanges();
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = false;
            }
            return Task.FromResult(res);
        }
        private string GenerateToken(AppUser user)
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
        public static string ComputeHmacSha256(string password, string username)
        {
            // Convert the username to bytes
            username = username.ToLower();
            byte[] keyBytes = Encoding.UTF8.GetBytes(username);

            // Create HMAC with SHA256
            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert bytes to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
