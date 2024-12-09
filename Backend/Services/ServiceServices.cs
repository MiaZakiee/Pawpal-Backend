using MongoDB.Driver; 
using PawpalBackend.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PawpalBackend.Services
{
    public class ServiceServices
    {
        private readonly IMongoCollection<Service> _servicesCollection;

        public ServiceServices(IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
            _servicesCollection = database.GetCollection<Service>(databaseSettings.Value.ServicesCollectionName);
        }

        // Find service by id
        public async Task<Service> GetServiceByIdAsync(string id) =>
            await _servicesCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        // Find all services
        public async Task<List<Service>> GetAllServicesAsync() =>
            await _servicesCollection.Find(_ => true).ToListAsync();

        // Create a service
        public async Task CreateServiceAsync(Service newService) =>
            await _servicesCollection.InsertOneAsync(newService);

        // Delete a service
        public async Task DeleteServiceAsync(string id) =>
            await _servicesCollection.DeleteOneAsync(s => s.Id == id);
    }
}