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
                Data = m.Data,
                Date = m.Date,
                Sder = m.Sender,
            }).ToList();
        }
        #endregion

        public bool InsertMessage(MessageDTO messageDTO)
        {
            return _repository.InsertMessage(new Message()
            {
                MessageCode = messageDTO.MCod,
                Data = messageDTO.Data,
                Date = messageDTO.Date,
                Sender = messageDTO.Sder

            });
        }

        
    }
}
