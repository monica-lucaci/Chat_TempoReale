using API_livechat.DTO;
using API_livechat.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API_livechat.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        #region Injections
        private IMongoCollection<ChatRoom> _chatRooms;
        private readonly ILogger _logger;
        private readonly MessageRepository _messageRepository;

        public ChatRoomRepository(IConfiguration configuration, ILogger<ChatRoomRepository> logger, MessageRepository messageRepository)
        {
            this._logger = logger;
            _messageRepository = messageRepository;
            string? localConnection = configuration.GetValue<string>("ConnectionStrings:MongoDbConnection");
            string? databaseName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

            MongoClient client = new MongoClient(localConnection);
            IMongoDatabase _database = client.GetDatabase(databaseName);
            _chatRooms = _database.GetCollection<ChatRoom>("ChatRoom");
        }
        #endregion

        public List<ChatRoom> GetChatRooms()
        {
            try
            {
                return _chatRooms.Find(cr => true).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return new List<ChatRoom>();
        }

        public ChatRoom? GetById(ObjectId chatRoomId)
        {
            try
            {
                return _chatRooms.Find(r => r.ChatRoomId == chatRoomId).ToList()[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public ChatRoom? GetByCode(string cr_code)
        {
            try
            {
                return _chatRooms.Find(r => r.ChatRoomCode == cr_code).ToList()[0];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public ChatRoom? GetChatRoom(string cr_code)
        {
            ChatRoom? cr = GetByCode(cr_code);
            if(cr == null) return null;

            cr.Messages = new List<Message>();
            cr.Messages = _messageRepository.GetMessages(cr.ChatRoomId);
            return cr;
        }

        public List<string>? GetUsersByChatRoom(string cr_code)
        {

            ChatRoom? cr = GetChatRoom(cr_code);
            if(cr != null) return cr.Users;
            return null;
        }

        public List<ChatRoom>? GetRoomByUser(string username)
        {
            try
            {
                List<ChatRoom> cr = new List<ChatRoom>();
                if(GetChatRooms() != null)
                {
                    foreach(ChatRoom r in GetChatRooms())
                    {
                        if (r.Users.Contains(username))
                        {
                            cr.Add(r);
                        }
                    }
                    return cr;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        //TO DO: SOSTITUISCI STRING USERNAME CON UN OGGETTO UTENTE
        public bool Create(ChatRoom chatRoom, string user)
        {
            try
            {
                if (_chatRooms.Find(cr => cr.Title == chatRoom.Title).ToList().Count > 0) return false;

                chatRoom.Users.Add(user);
                _chatRooms.InsertOne(chatRoom);
                _logger.LogInformation("Room creata con successo");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }
        //TO DO : SOSTITUISCI STRING USERNAME CON UN OGGETTO UTENTE
        public bool InsertUserIntoChatRoom(string username, string cr_code)
        {
            ChatRoom? cr_temp = GetByCode(cr_code);

            if (cr_temp != null)
            {
                cr_temp.Users.Add(username);

                var filter = Builders<ChatRoom>.Filter.Eq(cr => cr.ChatRoomId, cr_temp.ChatRoomId);
                try
                {
                    _chatRooms.ReplaceOne(filter, cr_temp);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return false;
        }

        public bool DeleteChatRoomByCode(string cr_code) 
        {
            try
            {
                var filter = Builders<ChatRoom>.Filter.Eq(c => c.ChatRoomCode, cr_code);
                if (filter != null)
                {
                    _chatRooms.DeleteOneAsync(filter);
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }
    }
}
