namespace API_livechat.DTO
{
    public class UserDTO : UserLoginDTO
    {
        public string Img { get; set; } = null!;
        public List<ChatRoomDTO> MyChatRooms { get; set; }
    }
}
