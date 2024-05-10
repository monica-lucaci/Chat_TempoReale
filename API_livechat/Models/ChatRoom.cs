using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API_livechat.Models
{
    public class ChatRoom
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId ChatRoomId { get; set; }

        [BsonElement("ChatRoomCode")]
        public string ChatRoomCode { get; set; } = Guid.NewGuid().ToString().ToUpper();

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("description")]
        public string? Description { get; set; }

        [BsonElement("image")]
        public string? Image { get; set; }

        public List<string> Users { get; set; } = new List<string>();
        
        [BsonRepresentation(BsonType.ObjectId)]
        public List<ObjectId> MessageRIF { get; set; } = new List<ObjectId>();
        public List<string> Messages { get; set; } = new List<string>(); 
    }
}
