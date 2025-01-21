using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MdAuthService.Models
{
    public class RegisterRequestBody
    {
        [Key]
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string Role { get; set; } = "user";
    }
}
