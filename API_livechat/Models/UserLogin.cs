namespace API_livechat.Models
{
    public class UserLogin
    {
        public int UserId { get; set; }
        public string? Code { get; set; } = Guid.NewGuid().ToString().ToUpper();
        public string Username { get; set; } = null!;
        public string Passwrd { get; set; } = null!;
        public string? UsRole { get; set; } = "USER";
    }
}
