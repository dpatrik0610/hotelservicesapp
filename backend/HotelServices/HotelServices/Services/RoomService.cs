using HotelServices.Database;
using HotelServices.Models;
using HotelServices.Services.Interfaces;
using MongoDB.Driver;

namespace HotelServices.Services
{
    public class RoomService : IRoomService
    {
        private readonly IMongoCollection<Room> _roomCollection;
        private readonly ILogger<RoomService> _logger;

        public RoomService(IMongoDatabaseProvider databaseProvider, ILogger<RoomService> logger)
        {
            var database = databaseProvider.GetDatabase();
            _roomCollection = database.GetCollection<Room>("Rooms");
            _logger = logger;
        }

        public async Task<Room> GetRoomByNumberAsync(int roomNumber)
        {
            try
            {
                return await _roomCollection.Find(room => room.RoomNumber == roomNumber).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching room data for room number {RoomNumber}.", roomNumber);
                throw new Exception("An error occurred while fetching room data.", ex);
            }
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            try
            {
                return await _roomCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all rooms.");
                throw new Exception("An error occurred while fetching all rooms.", ex);
            }
        }

        public async Task<List<Room>> GetAvailableRoomsAsync()
        {
            try
            {
                return await _roomCollection.Find(room => room.Availability).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching available rooms.");
                throw new Exception("An error occurred while fetching available rooms.", ex);
            }
        }

        public async Task AddRoomAsync(Room room)
        {
            try
            {
                await _roomCollection.InsertOneAsync(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the room.");
                throw new Exception("An error occurred while adding the room.", ex);
            }
        }

        public async Task<bool> DeleteRoomAsync(int roomNumber)
        {
            try
            {
                var result = await _roomCollection.DeleteOneAsync(room => room.RoomNumber == roomNumber);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the room with room number {RoomNumber}.", roomNumber);
                throw new Exception("An error occurred while deleting the room.", ex);
            }
        }

        public async Task<bool> UpdateRoomAsync(int roomNumber, Room newRoomData)
        {
            try
            {
                var result = await _roomCollection.ReplaceOneAsync(roomToFind => roomToFind.RoomNumber == roomNumber, newRoomData);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the room's data for room number {RoomNumber}.", roomNumber);
                throw new Exception("An error occurred while updating the room's data.", ex);
            }
        }

        public async Task<List<Room>> GetRoomsByNumbersAsync(List<int> roomNumbers)
        {
            try
            {
                var tasks = roomNumbers.Select(async roomNumber =>
                {
                    return await _roomCollection.Find(room => room.RoomNumber == roomNumber).FirstOrDefaultAsync();
                }).ToList();

                var rooms = await Task.WhenAll(tasks);
                return rooms.ToList();
            }
            catch (Exception ex)
            {
                var roomNumbersString = string.Join(", ", roomNumbers);
                _logger.LogError(ex, "An error occurred while getting rooms with numbers: {RoomNumbers}.", roomNumbersString);
                throw new Exception($"An error occurred while getting rooms with numbers: {roomNumbersString}. See logs for details.", ex);
            }
        }

        public async Task AddRoomsAsync(List<Room> rooms)
        {
            try { 
                await _roomCollection.InsertManyAsync(rooms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while Adding rooms.");
                throw new Exception("An error occurred while Adding rooms.");
            }
        }

        public async Task<bool> CheckRoomExistsAsync(int roomNumber)
        {
            try
            {
                var room = await _roomCollection.Find(r => r.RoomNumber == roomNumber).FirstOrDefaultAsync();
                return room != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the room exists.");
                throw new Exception("An error occurred while checking if the room exists.", ex);
            }
        }

        public async Task<List<bool>> CheckRoomsExistAsync(List<int> roomNumbers)
        {
            try
            {
                var results = new List<bool>();
                foreach (var roomNumber in roomNumbers)
                {
                    var roomExists = await CheckRoomExistsAsync(roomNumber);
                    results.Add(roomExists);
                }
                return results;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the rooms exist.");
                throw new Exception("An error occurred while checking if the rooms exist.", ex);
            }
        }
    }
}
