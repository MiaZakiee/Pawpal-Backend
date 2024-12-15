using MongoDB.Driver;
using PawpalBackend.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Linq;

namespace PawpalBackend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<Pet> _petsCollection;

        public UserService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
            _petsCollection = database.GetCollection<Pet>(databaseSettings.Value.PetsCollectionName);
        }

        // Find all users
        public async Task<List<User>> GetAsync() =>
            await _usersCollection.Find(_ => true).ToListAsync();

        // Find user by id
        public async Task<User> GetAsync(string id) =>
            await _usersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Find user by username
        public async Task<User> GetByUsername(string username) =>
            await _usersCollection.Find(x => x.Username == username).FirstOrDefaultAsync();

        // Find user by email
        public async Task<User> GetByEmail(string email) =>
            await _usersCollection.Find(x => x.Email == email).FirstOrDefaultAsync();

        // Find user by phonenumber
        public async Task<User> GetByPhoneNumber(string phoneNumber) =>
            await _usersCollection.Find(x => x.PhoneNumber == phoneNumber).FirstOrDefaultAsync();

        // Create user
        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        // Update user
        public async Task UpdateAsync(string id, User updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        // Remove user 
        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);

        // Byte array conversion
        public async Task<byte[]> ConvertToByteArrayAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        // Add service to user
        public async Task AddServiceToUserAsync(string userId, string serviceId)
        {
            var update = Builders<User>.Update.AddToSet(u => u.Services, serviceId);
            await _usersCollection.UpdateOneAsync(u => u.Id == userId, update);
        }

        // Remove service from user
        public async Task RemoveServiceFromUserAsync(string userId, string serviceId)
        {
            var update = Builders<User>.Update.Pull(u => u.Services, serviceId);
            await _usersCollection.UpdateOneAsync(u => u.Id == userId, update);
        }

        public async Task SaveProfilePictureAsync(string userId, IFormFile file)
        {
            var user = await _usersCollection.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.ProfilePicture = await ConvertToByteArrayAsync(file);
                await _usersCollection.ReplaceOneAsync(u => u.Id == userId, user);
            }
        }
    }
}
