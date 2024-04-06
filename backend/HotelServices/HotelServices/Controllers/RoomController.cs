using Microsoft.AspNetCore.Mvc;
using HotelServices.Models;
using HotelServices.Services.Interfaces;

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

        [HttpGet("byNumbers")]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms(IEnumerable<int> roomNumbers)
        {
            try
            {
                var rooms = await _roomService.GetRoomsByNumbersAsync(roomNumbers.ToList());
                if (rooms == null || !rooms.Any())
                {
                    return NotFound();
                }

                return Ok(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while retrieving the rooms: {0}", ex);
                return StatusCode(500, "An error occurred while retrieving the rooms.");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            try
            {
                var rooms = await _roomService.GetAllRoomsAsync();
                if (rooms == null || !rooms.Any())
                {
                    return NotFound();
                }

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
                if (room == null || room.RoomNumber == 0)
                {
                    return BadRequest("Room object cannot be null.");
                }

                var roomExist = await _roomService.GetRoomByNumberAsync(room.RoomNumber);
                if (roomExist != null) return StatusCode(403, "Room already exists by this number.");

                await _roomService.AddRoomAsync(room);
                _logger.LogInformation($"Added Room: {System.Text.Json.JsonSerializer.Serialize(room)}");
                return CreatedAtAction(nameof(GetRoom), new { roomNumber = room.RoomNumber }, room);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding the room: {0}", ex);
                return StatusCode(500, "An error occurred while adding the room.");
            }
        }

        [HttpPost("addMany")]
        public async Task<ActionResult<IEnumerable<Room>>> AddRooms(List<Room> rooms)
        {
            try
            {
                if (rooms == null || rooms.Count == 0)
                {
                    _logger.LogInformation($"Request Body: {System.Text.Json.JsonSerializer.Serialize(rooms)}");
                    return BadRequest("List of rooms cannot be null or empty.");
                }

                var roomNumbers = rooms.Select(r => r.RoomNumber).ToList();
                var checkRoomsExists = await _roomService.CheckRoomsExistAsync(roomNumbers);
                if (checkRoomsExists.Any(r => r))
                {
                    var existingRooms = await _roomService.GetRoomsByNumbersAsync(roomNumbers);
                    var existingRoomNumbers = existingRooms.Select(r => r.RoomNumber);
                    return StatusCode(403, $"Rooms already exist with the following numbers: {string.Join(", ", existingRoomNumbers)}");
                }

                await _roomService.AddRoomsAsync(rooms);
                return CreatedAtAction(nameof(GetRooms), new { roomNumbers = rooms.Select(r => r.RoomNumber) }, rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while adding the rooms: {0}", ex);
                return StatusCode(500, "An error occurred while adding the rooms.");
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
