using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace API_livechat.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId MessageId { get; set; }

        [BsonElement("messageCode")]
        public string MessageCode { get; set; } = Guid.NewGuid().ToString().ToUpper();

        [BsonElement("data")]
        public string? Data { get; set; }

        [BsonElement("date")]
        public DateTime Date { get; set; }

        [BsonElement("sender")]
        public string? Sender {  get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ChatRoomRIF { get; set; }

        public string? ChatRoomCode { get; set; }
    }
}
