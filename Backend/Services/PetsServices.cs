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
            await _petsCollection.Find(x => x.Owner == userId).ToListAsync();

        // find pet given pet id
        public async Task<Pet> GetPetByIdAsync(string id) =>
            await _petsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Create pet
        public async Task CreatePetAsync(Pet newPet) =>
            await _petsCollection.InsertOneAsync(newPet);

        // Update pet
        public async Task<Pet> UpdatePetAsync(string id, Pet updatedPet)
        {
            // Check if the pet exists before updating
            var filter = Builders<Pet>.Filter.Eq(pet => pet.Id, id);
            var existingPet = await _petsCollection.Find(filter).FirstOrDefaultAsync();

            if (existingPet == null)
            {
                throw new InvalidOperationException($"Pet with ID {id} not found.");
            }

            // Create the update definition, excluding _id from being updated
            var update = Builders<Pet>.Update
                .Set(pet => pet.Name, updatedPet.Name)
                .Set(pet => pet.Birthday, updatedPet.Birthday)
                .Set(pet => pet.Breed, updatedPet.Breed)
                .Set(pet => pet.ProfilePicture, updatedPet.ProfilePicture)
                .Set(pet => pet.Description, updatedPet.Description);

            // Perform the update
            var result = await _petsCollection.UpdateOneAsync(filter, update);

            var updatedPetFromDb = await _petsCollection.Find(filter).FirstOrDefaultAsync();
            return updatedPetFromDb;
        }

        // Remove pet
        public async Task RemovePetAsync(string id) =>
            await _petsCollection.DeleteOneAsync(x => x.Id == id);

    }
}
