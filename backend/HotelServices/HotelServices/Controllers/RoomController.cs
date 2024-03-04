using System;
using Microsoft.AspNetCore.Mvc;
using HotelServices.Services;
using HotelServices.Models;
using MongoDB.Driver;
using HotelServices.Services.Interfaces;

namespace HotelServices.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("{roomNumber}")]
        public async Task<ActionResult<Room>> GetRoom(int roomNumber)
        {
            try
            {
                var room = await _roomService.GetRoomByNumberAsync(roomNumber);
                if (room == null) return NotFound();

                return room;
            }
            catch (Exception ex)
            {
                // TODO: Implement logger here.
                return StatusCode(500, "An error occurred while retrieving the room.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();
                return rooms;
            }
            catch (Exception ex)
            {
                // TODO: Implement logger here.
                return StatusCode(500, "An error occurred while retrieving all rooms.");
            }
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<Room>>> GetAvailableRooms()
        {
            try
            {
                var availableRooms = await _roomService.GetAvailableRoomsAsync();
                return availableRooms;
            }
            catch (Exception ex)
            {
                // TODO: Implement logger here.
                return StatusCode(500, "An error occurred while retrieving available rooms.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Room>> AddRoom(Room room)
        {
            try
            {
                await _roomService.AddRoomAsync(room);
                return CreatedAtAction(nameof(GetRoom), new { roomNumber = room.RoomNumber }, room);
            }
            catch (Exception ex)
            {
                // TODO: Implement logger here.
                return StatusCode(500, "An error occurred while adding the room.");
            }
        }

        [HttpPut("{roomNumber}")]
        public async Task<IActionResult> UpdateRoom(int roomNumber, Room room)
        {
            try
            {
                if (roomNumber != room.RoomNumber) return BadRequest();

                var updated = await _roomService.UpdateRoomAsync(roomNumber, room);
                if (!updated) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: Implement logger here.
                return StatusCode(500, "An error occurred while updating the room.");
            }
        }

        [HttpDelete("{roomNumber}")]
        public async Task<IActionResult> DeleteRoom(int roomNumber)
        {
            try
            {
                var deleted = await _roomService.DeleteRoomAsync(roomNumber);
                if (!deleted) return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                // TODO: Implement logger here.
                return StatusCode(500, "An error occurred while deleting the room.");
            }
        }

    }
}
