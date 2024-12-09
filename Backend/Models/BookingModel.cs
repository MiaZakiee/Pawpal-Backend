using System.Security;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using PawpalBackend.Utils;

namespace PawpalBackend.Models {
    public class Booking {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("RecipientId")]
        public string RecipientId { get; set; }
        [BsonElement("ProviderId")]
        public string ProviderId { get; set; }
        [BsonElement("PetId")] 
        public string PetId { get; set; }
        [BsonElement("DateOfBooking")]
        public string DateOfBooking { get; set; }
        [BsonElement("Location")]
        public string Location { get; set; }
        [BsonElement("AddtionalInformation")]
        public string AddtionalInfo { get; set; }
        [BsonElement("TotalPrice")]
        public double TotalPrice { get; set; }
    }
}