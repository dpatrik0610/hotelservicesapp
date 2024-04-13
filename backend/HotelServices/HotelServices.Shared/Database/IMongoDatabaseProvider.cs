using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelServices.Shared.Database
{
    public interface IMongoDatabaseProvider
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
    }
}
