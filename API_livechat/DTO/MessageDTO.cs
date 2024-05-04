using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API_livechat.DTO
{
    public class MessageDTO
    {
        public ObjectId? MsId { get; set; }
        public string? MCod { get; set; }
        public string? Data { get; set; }
        public DateTime Date { get; set; } 
        public string? Sder { get; set; }
        public ObjectId? RRIF { get; set; }
    }
}
