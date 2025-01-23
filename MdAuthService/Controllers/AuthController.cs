using MdAuthService.Data;
using MdAuthService.Interfaces;
using MdAuthService.Models;
using Microsoft.AspNetCore.Authorization;
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

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRB content)
        {
            var result = await _authService.LoginAsync(content);
            if (result.LoggedIn)
            {
                return Ok(result);
            }
            return Unauthorized(new { message = result.Message });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRB content)
        {
            var result = await _authService.RegisterAsync(content);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(new { message = result.Message });
        }
        [HttpPost("reset-password")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRB content)
        {
            var result = await _authService.ResetPasswordAsync(content);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(new { message = result.Message });
        }
    }
}