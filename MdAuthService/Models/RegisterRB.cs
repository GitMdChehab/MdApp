using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MdAuthService.Models
{
    public class RegisterRB
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "user";
    }
}
