using API_livechat.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API_livechat.DTO
{
    public class ChatRoomDTO
    {
        public string? CRId { get; set; } = null!;
        public string CRCd { get; set; } = null!;
        public string Titl { get; set; } = null!;
        public string Desc { get; set; } = null!;
        public List<string> Usrs { get; set; } = new List<string>();
        public List<ObjectId> MRIF{ get; set; } = new List<ObjectId>();
        public List<MessageDTO>? Mges { get; set; }
    }
}
