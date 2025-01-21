using MdAuthService.Data;
using MdAuthService.Models;

namespace MdAuthService.Interfaces
{
    public interface IAuthService
    {
        Task<LoginDTO> LoginAsync(string username, string password);
        Task<RegisterDTO> RegisterAsync(RegisterRequestBody content);
        string ComputeHmacSha256(string password, string username);
        string GenerateToken(AppUser user);
    }

}
