using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
namespace HotelServices.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public int RoomNumber { get; set; }
        public bool Availability { get; set; } = true;
        public int Price { get; set; }
        public RoomType RoomType { get; set; }
        public string? Description { get; set; }
        public List<string>? Amenities { get; set; }
        public List<string>? images { get; set; }

    }
}
