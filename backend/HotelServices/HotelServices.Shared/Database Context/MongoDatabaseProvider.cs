using MongoDB.Driver;
using System;

namespace HotelServices.Shared.Database
{
    public class MongoDatabaseProvider : IMongoDatabaseProvider, IDisposable
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        public MongoDatabaseProvider(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string or database name cannot be null or empty.");
            }

            const int maxRetryAttempts = 3;
            const int delayBetweenRetriesMs = 2000;
            int retryAttempt = 0;

            while (true)
            {
                try
                {
                    MongoUrl url = new MongoUrl(connectionString);
                    MongoClientSettings settings = MongoClientSettings.FromUrl(url);
                    _client = new MongoClient(settings);
                    _database = _client.GetDatabase(url.DatabaseName);
                    break; // Exit the loop if connection is successful
                }
                catch (Exception ex)
                {
                    retryAttempt++;
                    if (retryAttempt > maxRetryAttempts)
                    {
                        throw new ApplicationException("Failed to establish connection to the MongoDB server after multiple retries.", ex);
                    }

                    // Log the retry attempt
                    Console.WriteLine($"Retry attempt {retryAttempt} failed. Retrying in {delayBetweenRetriesMs / 1000} seconds.");

                    // Wait before retrying
                    Thread.Sleep(delayBetweenRetriesMs);
                }
            }
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        public void Dispose()
        {
            _client.Cluster.Dispose();
        }
    }
}
