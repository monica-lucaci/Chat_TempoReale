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

        public MessageService(MessageRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region private metods
        List<MessageDTO> ConvertToMessagesDTO(List<Message> messages)
        {
            return messages.Select(m => new MessageDTO()
            {
                MCod = m.MessageCode,
                Data = m.Data,
                Date = m.Date,
                Sder = m.Sender
            }).ToList();
        }

        MessageDTO ConvertToMessageDTO(Message msg)
        {
            return new MessageDTO()
            {
                MCod = msg.MessageCode,
                Data = msg.Data,
                Date = msg.Date,
                Sder = msg.Sender
            };
        }

        Message ConvertToMessage(MessageDTO msgDTO)
        {
            return new Message()
            {
                MessageCode = msgDTO.MCod,
                Data = msgDTO.Data,
                Date = msgDTO.Date,
                Sender = msgDTO.Sder
            };
        }
        #endregion

        public bool InsertMessage(MessageDTO messageDTO)
        {
            return _repository.InsertMessage(ConvertToMessage(messageDTO));
        }
    }
}
