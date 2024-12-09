using MongoDB.Driver; 
using PawpalBackend.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PawpalBackend.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _usersCollection = database.GetCollection<User>(databaseSettings.Value.UsersCollectionName);
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
        public async Task<byte[]> ConvertToByteArrayAsync(IFormFile file) {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
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
