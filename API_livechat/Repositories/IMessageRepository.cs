using API_livechat.Models;
using MongoDB.Bson;

namespace API_livechat.Repositories
{
    public interface IMessageRepository
    {
        public Message? GetMessage(string ms_code);

        public bool InsertMessage(Message message);
    }
}
