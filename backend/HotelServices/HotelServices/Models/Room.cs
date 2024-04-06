using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace HotelServices.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = "Availability is required.")]
        public bool Availability { get; set; } = true;

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Room type is required.")]
        [EnumDataType(typeof(RoomType), ErrorMessage = "Invalid room type.")]
        public RoomType RoomType { get; set; }

        [MaxLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Amenities are required.")]
        public List<string> Amenities { get; set; } = new List<string>();

        public List<string> Images { get; set; } = new List<string>();
    }
}
