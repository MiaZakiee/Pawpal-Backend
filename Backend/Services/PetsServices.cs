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
    public class PetsService
    {
        private readonly IMongoCollection<Pet> _petsCollection;

        public PetsService(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _petsCollection = database.GetCollection<Pet>(databaseSettings.Value.PetsCollectionName);
        }

        // Find all pets given a user's id
        public async Task<List<Pet>> GetPetsForUserAsync(string userId) =>
            await _petsCollection.Find(x => x.Owner == new ObjectId(userId)).ToListAsync();

        // Create pet
        public async Task CreatePetAsync(Pet newPet) =>
            await _petsCollection.InsertOneAsync(newPet);

        // Update pet
        public async Task UpdatePetAsync(string id, Pet updatedPet) =>
            await _petsCollection.ReplaceOneAsync(x => x.Id == new ObjectId(id), updatedPet);

        // Remove pet
        public async Task RemovePetAsync(string id) =>
            await _petsCollection.DeleteOneAsync(x => x.Id == new ObjectId(id));

    }
}
