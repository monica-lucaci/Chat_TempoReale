using API_livechat.Models;
using MongoDB.Bson;

namespace API_livechat.Repositories
{
    public interface IChatRoomRepository
    {
        public List<ChatRoom> GetAll();
        public ChatRoom? GetById(ObjectId chatRoomId);
        public ChatRoom? GetChatRoom(ObjectId chatRoomId);
        public List<string>? GetUsersByChatRoom(ObjectId chatRoomId);
        public List<ChatRoom>? GetRoomByUser(string user);
        public bool Create(ChatRoom chatRoom, string user);
        public bool InsertUserIntoChatRoom(string user, ObjectId chatRoomId);
    }
}
