using MongoDB.Driver;

namespace HotelServices.Database
{
    public interface IMongoDatabaseProvider
    {
        IMongoDatabase GetDatabase();
    }

    public class MongoDatabaseProvider : IMongoDatabaseProvider
    {
        private readonly IMongoClient _client;
        private readonly string _databaseName;
        public MongoDatabaseProvider(string connectionString, string databaseName)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentException("Connection string or database name cannot be null or empty.");
            }

            _client = new MongoClient(connectionString);
            _databaseName = databaseName;
        }

        public IMongoDatabase GetDatabase()
        {
            try
            {
                // Check if the MongoDB server is reachable
                _client.ListDatabaseNames();

                return _client.GetDatabase(_databaseName);
            }
            catch (MongoException ex)
            {
                // TODO: Implement log here.
                throw new Exception("Failed to connect to the MongoDB server.", ex);
            }
        }
    }
}
