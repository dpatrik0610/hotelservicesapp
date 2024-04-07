using HotelServices.Database;
using HotelServices.Shared.Models;
using HotelServices.Services.Interfaces;
using MongoDB.Driver;

namespace HotelServices.Services
{
    public class UserReservationService : IUserReservationService
    {
        private readonly IMongoCollection<UserReservation> _userReservationCollection;
        private readonly ILogger<UserReservationService> _logger;

        public UserReservationService(IMongoDatabaseProvider databaseProvider, ILogger<UserReservationService> logger)
        {
            var database = databaseProvider.GetDatabase();
            _userReservationCollection = database.GetCollection<UserReservation>("UserReservations");
            _logger = logger;
        }

        public async Task<List<UserReservation>> GetAllUserReservationsAsync()
        {
            try
            {
                return await _userReservationCollection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all user reservations.");
                throw new Exception("An error occurred while getting all user reservations.");
            }
        }

        public async Task<UserReservation> GetUserReservationByIdAsync(string id)
        {
            try
            {
                return await _userReservationCollection.Find(ur => ur.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user reservation by ID: ${id}");
                throw new Exception($"An error occurred while getting user reservation by ID: ${id}");
            }
        }

        public async Task<List<UserReservation>> GetUserReservationsByUserIdAsync(string userId)
        {
            try
            {
                return await _userReservationCollection.Find(ur => ur.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user reservations by user ID: {userId}");
                throw new Exception($"An error occurred while getting user reservations by user ID: {userId}");
            }
        }

        public async Task<List<UserReservation>> GetUserReservationsByRoomIdAsync(string roomId)
        {
            try
            {
                return await _userReservationCollection.Find(ur => ur.RoomId == roomId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting user reservations by room ID: {roomId}");
                throw new Exception($"An error occurred while getting user reservations by room ID: {roomId}");
            }
        }

        public async Task CreateUserReservationAsync(UserReservation userReservation)
        {
            try
            {
                await _userReservationCollection.InsertOneAsync(userReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user reservation.");
                throw new Exception($"An error occurred while creating user reservation.");
            }
        }

        public async Task<bool> UpdateUserReservationAsync(string id, UserReservation userReservation)
        {
            try
            {
                var result = await _userReservationCollection.ReplaceOneAsync(ur => ur.Id == id, userReservation);
                return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,  $"An error occurred while updating user reservation with ID: {id}");
                throw new Exception($"An error occurred while updating user reservation with ID: {id}");
            }
        }

        public async Task<bool> DeleteUserReservationAsync(string id)
        {
            try
            {
                var result = await _userReservationCollection.DeleteOneAsync(ur => ur.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user reservation with ID: {id}");
                throw new Exception($"An error occurred while deleting user reservation with ID: {id}");
            }
        }
    }
}
