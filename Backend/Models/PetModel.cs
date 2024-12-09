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
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("breed")]
        public string Breed { get; set; }

        [BsonElement("age")]
        public int Age { get; set; }

        [BsonElement("owner")]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Owner { get; } // References the user's _id
    }

}
