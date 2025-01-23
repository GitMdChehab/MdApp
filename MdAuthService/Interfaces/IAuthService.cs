using MdAuthService.Data;
using MdAuthService.Models;

namespace MdAuthService.Interfaces
{
    public interface IAuthService
    {
        Task<LoginDTO> LoginAsync(LoginRB content);
        Task<RegisterDTO> RegisterAsync(RegisterRB content);
        Task<ResetPasswordDTO> ResetPasswordAsync(ResetPasswordRB content);
    }

}
