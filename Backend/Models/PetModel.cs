using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PawpalBackend.Utils;

namespace PawpalBackend.Models
{
    public class Pet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("Sex")]
        public string Sex { get; set; }

        [BsonElement("breed")]
        public string Breed { get; set; }

        [BsonElement("Birthday")]
        public string Birthday { get; set; }

        [BsonElement("owner")]
        public string Owner { get; set; } // Owner should be a user ID

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("profilePicture")]
        public string ProfilePicture { get; set; }


    }
}
