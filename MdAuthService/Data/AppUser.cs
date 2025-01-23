using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using RequiredAttribute = ServiceStack.DataAnnotations.RequiredAttribute;

namespace MdAuthService.Data
{
    public class AppUser
    {
        [Key]
        public long Id { get; set; }
        [Unique]
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [JsonIgnore]
        [Required]
        public string Password { get; set; }
        public string Role { get; set; } = "user";
    }
}