using API_livechat.DTO;
using API_livechat.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace API_livechat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        #region Injections
        private readonly loginContext _dbContext;
        private IMongoCollection<Message> _message;
        private readonly ILogger _logger;

        public MessageRepository(IConfiguration configuration, ILogger<ChatRoomRepository> logger, loginContext dbContext)
        {
            this._logger = logger;
            string? localConnection = configuration.GetValue<string>("ConnectionStrings:MongoDbConnection");
            string? databaseName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

            MongoClient client = new MongoClient(localConnection);
            IMongoDatabase _database = client.GetDatabase(databaseName);
            _message = _database.GetCollection<Message>("Message");
            _dbContext = dbContext;
        }
        #endregion

        public Message? GetMessage(string ms_code)
        {
            try
            {
                return _message.Find(m => m.MessageCode == ms_code).ToList()[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public List<Message> GetMessagesOfRoom(string cr_code) 
        {
            try
            {
                return _message.Find(m => m.ChatRoomCode == cr_code).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new List<Message>();
        }

        public List<Message> GetAllMessages()
        {
            try
            {
                return _message.Find(m => true).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new List<Message>();
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

        public bool DeleteMessage(Message message, string ms_code) {
            try
            {
                Message? msg_temp = GetMessage(ms_code);

                if(msg_temp != null)
                {
                    var filter = Builders<Message>.Filter.Eq(m => m.MessageId, msg_temp.MessageId);
                    _message.ReplaceOne(filter, msg_temp);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public List<Message>? GetMessByUser(string username)
        {
            try
            {
                //TO DO
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
