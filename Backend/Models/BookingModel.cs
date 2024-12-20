using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace PawpalBackend.Models {
    public class Booking {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("RecipientId")]
        public string RecipientId { get; set; }
        [BsonElement("ProviderId")]
        public string ProviderId { get; set; }
        [BsonElement("PetName")]
        public string PetName { get; set; }
        [BsonElement("DateOfBooking")]
        public string DateOfBooking { get; set; }
        [BsonElement("Location")]
        public string Location { get; set; }
        [BsonElement("Service")]
        // public List<string> Services { get; set; } = new List<string>();
        // Edited for ease because we are going to die if not
        public string Service { get; set; }
        [BsonElement("AddtionalInformation")]
        public string AddtionalInfo { get; set; }
        [BsonElement("TotalPrice")]
        public double TotalPrice { get; set; }
    }
}