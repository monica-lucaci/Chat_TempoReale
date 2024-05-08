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
                CRId = cr.ChatRoomId,
                CRCd = cr.ChatRoomCode,
                Titl = cr.Title,
                Desc = cr.Description,
                Usrs = cr.Users.ToList()
            }).ToList();
        }

        ChatRoomDTO ConvertToChatRoomDTO(ChatRoom chatRoom)
        {
            return new ChatRoomDTO()
            {
                CRId = chatRoom.ChatRoomId,
                CRCd = chatRoom.ChatRoomCode,
                Titl = chatRoom.Title,
                Desc = chatRoom.Description,
                Usrs = chatRoom.Users.ToList()
            };
        }

        ChatRoom ConvertToChatRoom(ChatRoomDTO chatRoomDTO) 
        {
            return new ChatRoom()
            {
                ChatRoomCode = chatRoomDTO.CRCd!,
                Title = chatRoomDTO.Titl,
                Description = chatRoomDTO.Desc,
                Users = chatRoomDTO.Usrs.ToList()
            };
        }

        public List<MessageDTO> ConvertToMessagesDTO(List<Message> msgs)
        {

            return msgs!.Select(m => new MessageDTO()
            {
                MsId = m.MessageId,
                MCod = m.MessageCode,
                Data = m.Data,
                Date = m.Date,
                Sder = m.Sender,
                RRIF = m.ChatRoomRIF,
            }).ToList();
        }

        public List<Message> ConvertToMessages(List<MessageDTO> msgsDTO)
        {
            return msgsDTO.Select(m => new Message()
            {
                MessageCode = m.MCod!,
                Data = m.Data,
                Date = m.Date,
                Sender = m.Sder
            }).ToList();
        }

        public string ConvertIdToCod(ChatRoomDTO chatRoomDTO)
        {
            if (chatRoomDTO == null) return $"Errore di conversione";
            return chatRoomDTO.CRCd!;
        }

        #endregion

        public IEnumerable<ChatRoomDTO>? GetAllChatRooms()
        {
            return ConvertToChatRoomsDTO(_repository.GetChatRooms());

            List<ChatRoom>? cr = _repository.GetChatRooms();
            if(cr != null) return ConvertToChatRoomsDTO(cr);
            return null;
        }

        public ChatRoomDTO? GetByCode(string cr_code)
        {
            ChatRoom? cr = _repository.GetChatRoom(cr_code);
            if (cr != null) return ConvertToChatRoomDTO(cr);
            return null;
        }

        public List<string>? GetUsersByChatRoom(string cr_code)
        {
            return _repository.GetUsersByChatRoom(cr_code);
        }

        public bool Insert(ChatRoomDTO chatRoomDTO, string user) {

           ChatRoom cr = new ChatRoom();
           cr.Description = chatRoomDTO.Desc;
           cr.Title = chatRoomDTO.Titl;
           return _repository.Create(cr, user);
        }

        public bool Delete(string cr_code, string user) 
        {
            List<ChatRoomDTO>? chatRoomDTOs = GetRoomsByUser(user);
            
            if(chatRoomDTOs != null)
            {
                foreach(ChatRoomDTO cr in chatRoomDTOs)
                {
                    if(cr_code == cr.CRCd)
                    {
                        if(_repository.DeleteChatRoomByCode(ConvertToChatRoom(cr).ChatRoomCode)) return true;
                    }
                }
            }
            return false;
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
                        CRCd = chatRoom.ChatRoomCode,
                        Titl = chatRoom.Title,
                        Desc = chatRoom.Description,
                        Usrs = chatRoom.Users,
                    };
                    chatRoomDTOs.Add(cr);
                }
            }
            return chatRoomDTOs;
        }

        public bool InsertUserIntoChatRoom(string username, string cr_code)
        {
            return _repository.InsertUserIntoChatRoom(username, cr_code);
        }

        public bool DeleteUserFromChatRoom(string username, string cr_code)
        {
            return _repository.DeleteUserFromChatRoom(username, cr_code);
        }
    }
}
