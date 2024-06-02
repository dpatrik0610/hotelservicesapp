using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using HotelServices.Shared.Models.Enums;

namespace HotelServices.Shared.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegDate { get; } = DateTime.UtcNow;

        [Required]
        [EnumDataType(typeof(RoleType), ErrorMessage = "Invalid role type.")]
        public RoleType? Role { get; set; } = RoleType.User;
    }
}