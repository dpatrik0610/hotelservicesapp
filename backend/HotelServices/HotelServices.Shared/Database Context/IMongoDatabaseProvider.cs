using MongoDB.Driver;

namespace HotelServices.Shared.Database
{
    public interface IMongoDatabaseProvider
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
