using API_livechat.Models;
using MongoDB.Bson;

namespace API_livechat.Repositories
{
    public interface IChatRoomRepository
    {
        public List<ChatRoom>? GetChatRooms();
        public ChatRoom? GetById(ObjectId chatRoomId);
        public ChatRoom? GetByCode(string cr_code);
        public ChatRoom? GetChatRoom(string cr_code);
        public List<string>? GetUsersByChatRoom(string cr_code);
        public List<ChatRoom>? GetRoomsByUser(string user);
        public bool Create(ChatRoom chatRoom, string user);
        public bool InsertUserIntoChatRoom(string user, string cr_code);
        public bool DeleteChatRoomByCode(string cr_code);
    }
}
