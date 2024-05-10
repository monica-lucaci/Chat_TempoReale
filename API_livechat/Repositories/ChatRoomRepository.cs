using API_livechat.DTO;
using API_livechat.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Runtime.ConstrainedExecution;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace API_livechat.Repositories
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        #region Injections
        private readonly loginContext _dbContext;
        private IMongoCollection<ChatRoom> _chatRooms;
        private readonly ILogger _logger;
        private readonly MessageRepository _messageRepository;

        public ChatRoomRepository(IConfiguration configuration, ILogger<ChatRoomRepository> logger, MessageRepository messageRepository, loginContext dbContext)
        {
            this._logger = logger;
            _dbContext = dbContext;
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
            if (cr == null) return null;
            return cr;
        }

        public List<string>? GetUsersByChatRoom(string cr_code)
        {

            ChatRoom? cr = GetChatRoom(cr_code);
            if(cr != null) return cr.Users;
            return null;
        }

        public List<ChatRoom>? GetRoomsByUser(string username)
        {
            try
            {
                List<ChatRoom> cr = new List<ChatRoom>();
                List<string> cr_codes = new List<string>();
                if (GetChatRooms() != null)
                {
                    foreach(ChatRoom r in GetChatRooms())
                    {
                        if (r.Users.Contains(username))
                        {
                            cr.Add(r);
                            cr_codes.Add(r.ChatRoomCode);
                        }
                    }
                    _dbContext.Users.FirstOrDefault(c => c.ChatRoomsCode == cr_codes);
                    _dbContext.SaveChanges();
                    return cr;
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }

        public bool Create(ChatRoom chatRoom, string user)
        {
            try
            {
                if (!UserReg(user)) return false;

                List<UserProfile> users = _dbContext.Users.ToList();

                foreach(UserProfile userProfile in users)
                {
                    if(userProfile.Username == user)
                    {
                        if (_chatRooms.Find(cr => cr.Title == chatRoom.Title).ToList().Count > 0) return false;
                        chatRoom.Users.Add(user);
                        _chatRooms.InsertOne(chatRoom);
                        _logger.LogInformation("Room creata con successo");
                        if (userProfile.ChatRoomsCode == null)
                        {
                            userProfile.ChatRoomsCode = new List<string>();
                        }
                        userProfile.ChatRoomsCode.Add(chatRoom.ChatRoomCode);
                        _dbContext.Users.SingleOrDefault(c => c.ChatRoomsCode == userProfile.ChatRoomsCode);
                        _dbContext.Users.Update(userProfile);
                        _dbContext.SaveChanges();
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

        public bool InsertUserIntoChatRoom(string username, string cr_code)
        {
            try
            {
                if (UserReg(username))
                {
                    if (UserIsInChatRoom(username, cr_code)) return false;
                    
                    ChatRoom? cr_temp = GetByCode(cr_code);
                    if (cr_temp != null)
                    {
                        cr_temp.Users.Add(username);
                        var filter = Builders<ChatRoom>.Filter.Eq(cr => cr.ChatRoomId, cr_temp.ChatRoomId);
                        _chatRooms.ReplaceOne(filter, cr_temp);

                        List<UserProfile> users = _dbContext.Users.ToList();
                        foreach (UserProfile userProfile in users)
                        {
                            if (userProfile.Username == username)
                            {
                                if (userProfile.ChatRoomsCode == null)
                                {
                                    userProfile.ChatRoomsCode = new List<string>();
                                }
                                userProfile.ChatRoomsCode.Add(cr_temp.ChatRoomCode);
                                _dbContext.Users.SingleOrDefault(c => c.ChatRoomsCode == userProfile.ChatRoomsCode);
                                _dbContext.Users.Update(userProfile);
                                _dbContext.SaveChanges();
                                return true;
                            }
                        }

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

        public bool DeleteUserFromChatRoom(string username, string cr_code)
        {
            try
            {
                if(UserReg(username))
                {
                    if (!UserIsInChatRoom(username, cr_code)) return false;

                    foreach (string usrsInCr in GetChatRoom(cr_code)!.Users)
                    {
                        //se la chatroom contiene l'utente passato in input allora lo elimino dalla chatroom
                        if (usrsInCr == username)
                        {
                            ChatRoom? cr_temp = GetChatRoom(cr_code);
                            if (cr_temp != null)
                            {
                                List<UserProfile> users = _dbContext.Users.ToList();
                                foreach (UserProfile userProfile in users)
                                {
                                    if (userProfile.Username == username)
                                    {
                                        if (userProfile.ChatRoomsCode != null && userProfile.ChatRoomsCode.Contains(cr_code))
                                        {
                                            userProfile.ChatRoomsCode.Remove(cr_temp.ChatRoomCode);
                                            _dbContext.Users.SingleOrDefault(c => c.ChatRoomsCode == userProfile.ChatRoomsCode);
                                            _dbContext.Users.Update(userProfile);
                                            _dbContext.SaveChanges();
                                        }
                                    }
                                }

                                cr_temp.Users.Remove(username);
                                var filter = Builders<ChatRoom>.Filter.Eq(cr => cr.ChatRoomId, cr_temp.ChatRoomId);
                                _chatRooms.ReplaceOne(filter, cr_temp);

                                return true;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public bool DeleteUserNotRegFromChatRoom(string username, string cr_code)
        {
            try
            {
                ChatRoom? ctr = GetByCode(cr_code);
                if (ctr != null)
                {
                    List<UserProfile> users = _dbContext.Users.ToList();
                    foreach (UserProfile userProfile in users)
                    {
                        if (userProfile.Username == username)
                        {
                            if (userProfile.ChatRoomsCode != null && userProfile.ChatRoomsCode.Contains(cr_code))
                            {
                                userProfile.ChatRoomsCode.Remove(ctr.ChatRoomCode);
                                _dbContext.Users.SingleOrDefault(c => c.ChatRoomsCode == userProfile.ChatRoomsCode);
                                _dbContext.Users.Update(userProfile);
                                _dbContext.SaveChanges();
                            }
                        }
                    }

                    ctr.Users.Remove(username);
                    var filter = Builders<ChatRoom>.Filter.Eq(cr => cr.ChatRoomId, ctr.ChatRoomId);
                    _chatRooms.ReplaceOne(filter, ctr);
                    return true;
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public bool DeleteUserFromAllChatRooms(string username)
        {
            try
            {
                if (UserReg(username))
                {
                    foreach (ChatRoom ctr in GetChatRooms())
                    {
                        //se la chatroom contiene l'utente passato in input allora lo elimino dalla chatroom
                        if (UserIsInChatRoom(username,ctr.ChatRoomCode))
                        {
                            if (ctr != null)
                            {
                                List<UserProfile> users = _dbContext.Users.ToList();
                                foreach (UserProfile userProfile in users)
                                {
                                    if (userProfile.Username == username)
                                    {
                                        if (userProfile.ChatRoomsCode != null && userProfile.ChatRoomsCode.Contains(ctr.ChatRoomCode))
                                        {
                                            userProfile.ChatRoomsCode.Remove(ctr.ChatRoomCode);
                                            _dbContext.Users.SingleOrDefault(c => c.ChatRoomsCode == userProfile.ChatRoomsCode);
                                            _dbContext.Users.Update(userProfile);
                                            _dbContext.SaveChanges();
                                        }
                                    }
                                }

                                ctr.Users.Remove(username);
                                var filter = Builders<ChatRoom>.Filter.Eq(cr => cr.ChatRoomId, ctr.ChatRoomId);
                                _chatRooms.ReplaceOne(filter, ctr);
                                return true;
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }
        public bool DeleteChatRoomByCode(string cr_code)
        {
            try
            {
                List<UserProfile> users = _dbContext.Users.ToList();
                foreach (UserProfile userProfile in users)
                {
                    if (userProfile.ChatRoomsCode != null )
                    {
                        foreach (string crc in userProfile.ChatRoomsCode)
                        {
                            if (userProfile.ChatRoomsCode.Contains(cr_code))
                            {
                                userProfile.ChatRoomsCode.Remove(cr_code);
                                _dbContext.Users.Update(userProfile);
                                _dbContext.SaveChanges();
                                break;
                            }
                        }
                    }
                }

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

        /// <summary>
        /// Controllo se l'utente ha già effettuato una registrazione
        /// </summary>
        /// <param name="username"></param>
        /// <returns>true: utente registrato, altrimenti false</returns>
        public bool UserReg(string username)
        {
            List<UserProfile> users = _dbContext.Users.ToList();
            foreach (UserProfile userProfile in users)
            {
                if (userProfile.Username == username) return true;
            }
            return false;
        }

        /// <summary>
        /// Controllo se l'utente è nella chat room di cui codice passato in input
        /// </summary>
        /// <param name="username"></param>
        /// <param name="cr_code"></param>
        /// <returns>true: utente in chat room, altrimenti false</returns>
        public bool UserIsInChatRoom(string username, string cr_code)
        {
            foreach (string usrsInCr in GetChatRoom(cr_code)!.Users)
            {
                if (usrsInCr == username) return true;
            }

            return false;
        }

        /// <summary>
        /// controlla le stanze vuote, se esistono le elimina
        /// </summary>
        /// <param name="cr_code"></param>
        /// <returns>true, se sono state trovate ed eliminate o se non ci sono problemi, false altrimenti</returns>

        public bool ChechEmptyChatrooms()
        {
            try
            {
                foreach (ChatRoom ctr in GetChatRooms())
                {
                    if (ctr.Users.Count == 0) return DeleteChatRoomByCode(ctr.ChatRoomCode);
                }
            }
            catch(Exception ex)
            { 
                _logger.LogError(ex.Message); 
                return false;
            }
            return true;
        }

        /// <summary>
        /// controllo se ci sono utenti nella stanza che non sono più registrati
        /// </summary>
        /// <returns>true, se sono stati trovati ed eliminati o se non ci sono problemi, false altrimenti</returns>
        public bool CheckUserInChatRooms()
        {
            try
            {
                ChechEmptyChatrooms();
                foreach (ChatRoom ctr in GetChatRooms())
                {
                    foreach (string usr in ctr.Users)
                    {
                        if (!UserReg(usr) && UserIsInChatRoom(usr, ctr.ChatRoomCode))
                        {
                            return DeleteUserNotRegFromChatRoom(usr, ctr.ChatRoomCode) && ChechEmptyChatrooms();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
    }
}
