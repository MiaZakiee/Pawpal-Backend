using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PawpalBackend.Models
{
    public class Service
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("ServiceName")]
        public string ServiceName { get; set; }
    }
}