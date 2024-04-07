using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace HotelServices.Shared.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Nickname is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Nickname must be between 3 and 50 characters")]
        public string? Nickname { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string? Password { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegDate { get; } = DateTime.UtcNow;
    }
}