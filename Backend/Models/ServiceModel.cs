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

        [BsonElement("ServiceDescription")]
        public string ServiceDescription { get; set; }

        [BsonElement("ServicePrice")]
        public double ServicePrice { get; set; }

        [BsonElement("ServiceOwner")]
        public string ServiceOwner { get; set; }

        [BsonElement("ServicePicture")]
        public string ServicePicture { get; set; }
    }
}