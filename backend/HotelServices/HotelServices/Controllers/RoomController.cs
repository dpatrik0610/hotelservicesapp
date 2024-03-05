using Microsoft.AspNetCore.Mvc;
using HotelServices.Models;
using HotelServices.Services.Interfaces;
using HotelServices.Services;

namespace HotelServices.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly ILogger<RoomController> _logger;
        public RoomController(IRoomService roomService, ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _logger = logger;
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
                _logger.LogError("An error occurred while retrieving the room: {0}", ex);
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
                _logger.LogError("An error occurred while retrieving all rooms: {0}", ex);
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
                _logger.LogError("An error occurred while retrieving available rooms: {0}", ex);
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
                _logger.LogError("An error occurred while adding the room: {0}", ex);
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
                _logger.LogError("An error occurred while updating the room: {0}", ex);
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
                _logger.LogError("An error occurred while deleting the room: {0}", ex);
                return StatusCode(500, "An error occurred while deleting the room.");
            }
        }

    }
}
