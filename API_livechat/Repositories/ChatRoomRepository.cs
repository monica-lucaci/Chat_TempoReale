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

        public ChatRoomRepository(IConfiguration configuration, ILogger<ChatRoomRepository> logger, MessageRepository repo)
        {
            this._logger = logger;
            _messageRepository = repo;
            string? connessioneLocale = configuration.GetValue<string>("ConnectionStrings:MongoDbConnection");
            string? databaseName = configuration.GetValue<string>("ConnectionStrings:MongoDbName");

            MongoClient client = new MongoClient(connessioneLocale);
            IMongoDatabase _database = client.GetDatabase(databaseName);
            _chatRooms = _database.GetCollection<ChatRoom>("Room");

        }

        #endregion

        public List<ChatRoom> GetAll()
        {
            return _chatRooms.Find(cr => true).ToList();
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

        public ChatRoom? GetChatRoom(ObjectId chatRoomId)
        {
            ChatRoom? cr = GetById(chatRoomId);
            
            if(cr == null) return null;

            cr.Messages = new List<Message>();
            cr.Messages = _messageRepository.GetMessages(chatRoomId);
            return cr;
        }

        public List<string>? GetUsersByChatRoom(ObjectId chatRoomId)
        {
            ChatRoom? cr = GetChatRoom(chatRoomId);
            if(cr != null) return cr.Users;
            return null;
        }

        public List<ChatRoom>? GetRoomByUser(string username)
        {
            List<ChatRoom> cr = new List<ChatRoom>();
            foreach(ChatRoom r in GetAll())
            {
                if (r.Users.Contains(username))
                {
                    cr.Add(r);
                }
            }
            return cr;
        }

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

        public bool InsertUserIntoChatRoom(string username, ObjectId chatRoomId)
        {
            ChatRoom? cr_temp = GetById(chatRoomId);

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

    }
}
