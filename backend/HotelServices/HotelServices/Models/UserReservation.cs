using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelServices.Models
{
    public class UserReservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "User ID is required")]
        public required string UserId { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Required(ErrorMessage = "Room ID is required")]
        public string? RoomId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReservationDate { get; } = DateTime.UtcNow;
    }
}
