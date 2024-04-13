using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace Hotelservices.UserAuth.IdentityModels
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<ObjectId> { };

    [CollectionName("Roles")]
    public class ApplicationRole : MongoIdentityRole<ObjectId> { };
}