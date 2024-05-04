using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API_livechat.DTO
{
    public class MessageDTO
    {
        public string MCod { get; set; } = null!;
        public string Data { get; set; } = null!;
        public DateTime Date { get; set; }
        public string Sder { get; set; } = null!;
        public ObjectId RRIF { get; set; }
    }
}
