using API_livechat.Models;
using MongoDB.Bson;

namespace API_livechat.Repositories
{
    public interface IMessageRepository
    {
        public List<Message> GetMessages(ObjectId roomRif);
        public bool InsertMessage(Message message);
    }
}
