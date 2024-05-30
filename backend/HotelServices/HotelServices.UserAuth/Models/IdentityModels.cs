using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace Hotelservices.UserAuth.IdentityModels
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<ObjectId> {

        [BsonRepresentation(BsonType.ObjectId)]
        public override ObjectId Id { get; set; }

    };

    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityRole<ObjectId> { };
}