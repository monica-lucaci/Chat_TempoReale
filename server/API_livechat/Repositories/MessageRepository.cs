using API_livechat.DTO;
using API_livechat.Models;
using API_livechat.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace API_livechat.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        #region Injections
        private readonly DbLoginContext _dbContext;
        private IMongoCollection<Message> _message;
        private readonly ILogger _logger;

        public MessageRepository(IConfiguration configuration, ILogger<ChatRoomRepository> logger, DbLoginContext dbContext)
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
                if (UserReg(message.Sender!))
                {
                    _message.InsertOne(message);
                    _logger.LogInformation("Messaggio inserito");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public bool DeleteMessage(string ms_code, string username)
        {
            try
            {
                if (GetMessageUserSender(ms_code, username) != null)
                {
                    var filter = Builders<Message>.Filter.Eq(m => m.MessageCode, ms_code);
                    if (filter != null)
                    {
                        _message.DeleteOneAsync(filter);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public bool DeleteMessagesOfUser(string username)
        {
            try
            {
                List<Message>? messages = GetMessByUser(username);

                if (messages == null || messages.Count == 0) return true;

                int i = 0;
                while ((messages != null || messages!.Count > 0) && i <= messages.Count)
                {
                    var filter = Builders<Message>.Filter.Eq(m => m.Sender, username);
                    if (filter != null)
                    {
                        _message.DeleteOneAsync(filter);
                        if (i > messages.Count) return true;
                        messages.Remove(messages[i]);
                        if (messages == null || messages.Count == 0) return true;
                        i++;
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public bool DeleteMessagesOfRoom(string cr_code)
        {
            try
            {
                List<Message>? messages = GetMessagesOfRoom(cr_code);

                if (messages == null || messages.Count == 0) return true;

                int i = 0;
                while ((messages != null || messages!.Count > 0) && i <= messages.Count)
                {
                    var filter = Builders<Message>.Filter.Eq(m => m.ChatRoomCode, cr_code);
                    if (filter != null)
                    {
                        if (messages == null || messages.Count == 0) return true;
                        _message.DeleteOneAsync(filter);
                        messages.Remove(messages[messages.Count - 1]);
                        if (i > messages.Count) return true;
                    }
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
                return _message.Find(m => m.Sender == username).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Controllo se l'utente ha già effettuato una registrazione
        /// </summary>
        /// <param name="username"></param>
        /// <returns>true: utente registrato, altrimenti false</returns>
        public bool UserReg(string username)
        {
            List<UserProfile> users = _dbContext.UserProfiles.ToList();
            foreach (UserProfile userProfile in users)
            {
                if (userProfile.Username == username) return true;
            }
            return false;
        }

        /// <summary>
        /// Controllo se l'utente è il sender del messaggio e restituico il messaggio
        /// </summary>
        /// <param name="username"></param>
        public Message? GetMessageUserSender(string ms_code, string username)
        {
            if (UserReg(username))
            {
                return _message.Find(m => m.Sender == username && m.MessageCode == ms_code).ToList()[0];
            }
            return null;
        }

        public bool CheckUserSender(string ms_code, string username)
        {
            if (GetMessageUserSender(ms_code, username) != null) return true;
            return false;
        }

        public bool UpdateMessage(string ms_code, string textMessage)
        {
            try
            {
                var filter = Builders<Message>.Filter.Eq(m => m.MessageCode, ms_code);
                var update = Builders<Message>.Update.Set(m => m.Data, textMessage);

                _message.UpdateOneAsync(filter, update);
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