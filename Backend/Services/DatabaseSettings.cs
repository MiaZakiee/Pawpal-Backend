namespace PawpalBackend.Models
{
        public class DatabaseSettings
        {
                public string ConnectionString { get; set; } = null!;
                public string DatabaseName { get; set; } = null!;
                public string UsersCollectionName { get; set; } = null!;

                public string PetsCollectionName { get; set; } = null!;
                public string BookingsCollectionName { get; set; } = null!;
                public string ServicesCollectionName { get; set; } = null!;
        }
}
