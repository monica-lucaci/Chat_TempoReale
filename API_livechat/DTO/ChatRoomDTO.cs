using API_livechat.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API_livechat.DTO
{
    public class ChatRoomDTO
    {
        public ObjectId? CRId { get; set; }
        public string? CRCd { get; set; }
        public string? Titl { get; set; }
        public string? Desc { get; set; }
        public string? CRImg { get; set; }
        public List<string> Usrs { get; set; } = new List<string>();
        public List<ObjectId> MRIF{ get; set; } = new List<ObjectId>();
        public List<string> Mges { get; set; } = new List<string>();
    }
}
