using API_livechat.DTO;
using API_livechat.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API_livechat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        #region Injections
        private IMongoCollection<Message> _message;
        private readonly ILogger _logger;

        public MessageRepository(IConfiguration configuration, ILogger<ChatRoomRepository> logger)
        {
            this._logger = logger;

            string? connessioneLocale = configuration.GetValue<string>("ConnectionStrings:MongoDbConnection");
            string? databaseName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

            MongoClient client = new MongoClient(connessioneLocale);
            IMongoDatabase _database = client.GetDatabase(databaseName);
            _message = _database.GetCollection<Message>("Message");
        }
        #endregion

        public List<Message> GetMessages(ObjectId roomRif)
        {
            var filter = Builders<Message>.Filter.Eq(cr => cr.ChatRoomRIF, roomRif);
            return _message.Find(filter).ToList();
        }

        public bool InsertMessage(Message message)
        {
            try
            {
                _message.InsertOne(message);
                _logger.LogInformation("Messaggio inserito");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

    }
}
