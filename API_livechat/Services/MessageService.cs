using API_livechat.DTO;
using API_livechat.Models;
using API_livechat.Repositories;
using MongoDB.Bson;

namespace API_livechat.Services
{
    public class MessageService
    {
        #region repository
        private readonly MessageRepository _repository;
        private readonly ChatRoomRepository _roomRepository;

        public MessageService(MessageRepository repository,ChatRoomRepository roomRepository)
        {
            _repository = repository;
            _roomRepository = roomRepository;
        }
        #endregion

        #region private metods
        List<MessageDTO> ConvertToMessagesDTO(List<Message> messages)
        {
            return messages.Select(m => new MessageDTO()
            {
                MsId = m.MessageId,
                MCod = m.MessageCode,
                Data = m.Data,
                Date = m.Date,
                Sder = m.Sender,
                RRIF = m.ChatRoomRIF
            }).ToList();
        }

        MessageDTO ConvertToMessageDTO(Message msg)
        {
            return new MessageDTO()
            {
                MsId = msg.MessageId,
                MCod = msg.MessageCode,
                Data = msg.Data,
                Date = msg.Date,
                Sder = msg.Sender,
                RRIF = msg.ChatRoomRIF
            };
        }

        Message ConvertToMessage(MessageDTO msgDTO)
        {
            return new Message()
            {
                MessageCode = msgDTO.MCod!,
                Data = msgDTO.Data,
                Date = msgDTO.Date,
                Sender = msgDTO.Sder
            };
        }
        #endregion

        public bool InsertMessage(MessageDTO messageDTO, ObjectId chatRoomId)
        {
            if (_roomRepository.GetById(chatRoomId) != null) 
            {
                messageDTO.RRIF = chatRoomId;
                return _repository.InsertMessage(ConvertToMessage(messageDTO));
            }
            return false;
        }
    }
}
