using MongoDB.Driver; 
using PawpalBackend.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawpalBackend.Services
{
    public class BookingService
    {
        private readonly IMongoCollection<Booking> _bookingCollection;
        
        public BookingService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _bookingCollection = database.GetCollection<Booking>(databaseSettings.Value.BookingsCollectionName);
        }

        // Get All Bookings
        public async Task<List<Booking>> GetBookingListAsync() =>
            await _bookingCollection.Find(_ => true).ToListAsync();

        // Create Booking
        public async Task CreateBookingAsync(Booking newBooking) =>     
            await _bookingCollection.InsertOneAsync(newBooking);

        // Get Speciifc Booking
        public async Task<Booking> GetBookingByIdAsync(string id) =>
            await _bookingCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Get Bookings By Recipient
        public async Task<List<Booking>> GetBookingByRecipientIdAsync(string recipientId) =>
            await _bookingCollection.Find(x => x.RecipientId == recipientId).ToListAsync();

        // Update Booking
        public async Task UpdateBookingAsync(string id, Booking updatedBooking) =>
            await _bookingCollection.ReplaceOneAsync(x => x.Id == id, updatedBooking);

        // Remove Booking
        public async Task RemoveBookingAsync(string id) =>
            await _bookingCollection.DeleteOneAsync(x => x.Id == id);

        // Add service to booking
        public async Task AddServiceToBookingAsync(string bookingId, string serviceId)
        {
            var update = Builders<Booking>.Update.Set(u => u.Service, serviceId);
            await _bookingCollection.UpdateOneAsync(u => u.Id == bookingId, update);
        }

        // Remove service from booking
        public async Task RemoveServiceFromBookingAsync(string bookingId)
        {
            var update = Builders<Booking>.Update.Set(u => u.Service, null);
            await _bookingCollection.UpdateOneAsync(u => u.Id == bookingId, update);
        }

    }
    
}
