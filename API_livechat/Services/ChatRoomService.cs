using API_livechat.DTO;
using API_livechat.Models;
using API_livechat.Repositories;
using MongoDB.Bson;

namespace API_livechat.Services
{
    public class ChatRoomService
    {
        #region repository
        private readonly ChatRoomRepository _repository;

        public ChatRoomService(ChatRoomRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region private metods
        List<ChatRoomDTO> ConvertToChatRoomsDTO(List<ChatRoom> chatRooms)
        {
            return chatRooms.Select(cr => new ChatRoomDTO()
            {
                CRCD = cr.ChatRoomCode,
                Titl = cr.Title,
                Desc = cr.Description,
                Usrs = cr.Users.ToList(),
                Mges = ConvertToMessagesDTO(cr.Messages.ToList())
            }).ToList();
        }

        ChatRoomDTO ConvertToChatRoomDTO(ChatRoom chatRoom)
        {
            return new ChatRoomDTO()
            {
                CRCD = chatRoom.ChatRoomCode,
                Titl = chatRoom.Title,
                Desc = chatRoom.Description,
                Usrs = chatRoom.Users.ToList(),
                Mges = ConvertToMessagesDTO(chatRoom.Messages.ToList())
            };
        }

        public List<MessageDTO> ConvertToMessagesDTO(List<Message> msgs)
        {
            return msgs.Select(m => new MessageDTO()
            {
                MCod = m.MessageCode,
                Data = m.Data,
                Date = m.Date,
                Sder = m.Sender,
                RRIF = m.ChatRoomRIF
            }).ToList();
        }
        #endregion

        public IEnumerable<ChatRoomDTO> GetAll()
        {
            return ConvertToChatRoomsDTO(_repository.GetAll());
        }

        public ChatRoomDTO? GetById(ObjectId chatRoomId)
        {
            ChatRoom? cr = _repository.GetChatRoom(chatRoomId);
            if(cr != null) return ConvertToChatRoomDTO(cr);
            return null;
        }
        public List<string>? GetUsersByChatRoom(ObjectId chatRoomId)
        {
            return _repository.GetUsersByChatRoom(chatRoomId);
        }

        public bool Insert(ChatRoomDTO chatRoomDTO, string user) {
            ChatRoom cr = new ChatRoom();
            cr.Description = chatRoomDTO.Desc;
            cr.Title = chatRoomDTO.Titl;
            return _repository.Create(cr, user);
        }

        public List<ChatRoomDTO>? GetRoomsByUser(string username)
        {
            List<ChatRoom>? chats = _repository.GetRoomByUser(username);
            List<ChatRoomDTO>? chatRoomDTOs = new List<ChatRoomDTO>();

            if(chats != null)
            {
                foreach(ChatRoom chatRoom in chats)
                {
                    ChatRoomDTO cr = new ChatRoomDTO()
                    {
                        CRCD = chatRoom.ChatRoomCode,
                        Titl = chatRoom.Title,
                        Desc = chatRoom.Description,
                        Usrs = chatRoom.Users,
                    };
                    chatRoomDTOs.Add(cr);
                }
            }
            return chatRoomDTOs;
        }

        public bool InsertUserIntoChatRoom(string username, ObjectId chatRoomId)
        {
            return _repository.InsertUserIntoChatRoom(username, chatRoomId);
        }
    }
}
