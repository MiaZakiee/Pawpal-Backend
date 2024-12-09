using MongoDB.Driver;
using PawpalBackend.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // Create user
        public async Task CreateAsync(User newUser) =>
            await _usersCollection.InsertOneAsync(newUser);

        // Update user
        public async Task UpdateAsync(string id, User updatedUser) =>
            await _usersCollection.ReplaceOneAsync(x => x.Id == id, updatedUser);

        // Remove user 
        public async Task RemoveAsync(string id) =>
            await _usersCollection.DeleteOneAsync(x => x.Id == id);
    }
}
