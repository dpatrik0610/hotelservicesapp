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
            _client = new MongoClient(connectionString);
            _databaseName = databaseName;
        }

        public IMongoDatabase GetDatabase()
        {
            return _client.GetDatabase(_databaseName);
        }
    }
}
