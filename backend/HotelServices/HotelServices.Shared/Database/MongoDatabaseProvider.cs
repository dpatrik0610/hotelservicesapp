using MongoDB.Driver;


namespace HotelServices.Shared.Database
{
    public class MongoDatabaseProvider : IMongoDatabaseProvider
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDatabaseProvider(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string or database name cannot be null or empty.");
            }

            MongoUrl url = new MongoUrl(connectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(url);
            _client = new MongoClient(settings);
            _database = _client.GetDatabase(url.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
