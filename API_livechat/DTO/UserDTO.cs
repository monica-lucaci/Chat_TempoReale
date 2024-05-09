namespace API_livechat.DTO
{
    public class UserDTO : UserLoginDTO
    {
        public string? Img { get; set; }
        public List<string>? CRCode { get; set; }

    }
}
