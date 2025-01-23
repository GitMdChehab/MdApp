namespace MdAuthService.Models
{
    public class LoginDTO
    {
        public string Token { get; set; }
        public bool LoggedIn { get; set; }
        public string Message { get; internal set; }
    }
}