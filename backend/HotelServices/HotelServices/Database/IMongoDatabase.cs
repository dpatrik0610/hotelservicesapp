using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

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
        private readonly ILogger _logger;

        public MongoDatabaseProvider(string connectionString, string databaseName, ILogger logger)
        {
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentException("Connection string or database name cannot be null or empty.");
            }

            var settings = MongoClientSettings.FromConnectionString(connectionString);
            _client = new MongoClient(settings);
            _databaseName = databaseName;
            _logger = logger;
        }

        public IMongoDatabase GetDatabase()
        {
            try
            {
                // Check if the MongoDB server is reachable
                _client.ListDatabaseNames();

                return _client.GetDatabase(_databaseName);
            }
            catch
            {
                _logger.LogError($"Failed to connect to the MongoDB server.");
            }

            _logger.LogError($"Failed to connect to the MongoDB server.");
            throw new Exception($"Failed to connect to the MongoDB server.");
        }
    }
}
