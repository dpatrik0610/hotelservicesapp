using HotelServices.Database;
using HotelServices.Shared.Models;
using HotelServices.Services.Interfaces;
using MongoDB.Driver;

namespace HotelServices.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly ILogger<UserService> _logger;

        public UserService(IMongoDatabaseProvider databaseProvider, ILogger<UserService> logger)
        {
            var database = databaseProvider.GetDatabase();
            _usersCollection = database.GetCollection<User>("Users");
            _logger = logger;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                return await _usersCollection.Find(user => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all users.");
                throw new Exception("An error occurred while retrieving all users.");
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                return await _usersCollection.Find(user => user.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving user by ID: {id}.");
                throw new Exception($"An error occurred while retrieving user by ID: {id}.");
            }
        }

        public async Task CreateUserAsync(User user)
        {
            try
            {
                await _usersCollection.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                throw new Exception($"An error occurred while creating a new user: {user}");
            }
        }

        public async Task<bool> UpdateUserAsync(string id, User user)
        {
            try
            {
                var result = await _usersCollection.ReplaceOneAsync(u => u.Id == id, user);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID: {id}.");
                throw new Exception($"An error occurred while updating user with ID: {id}.");
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            try
            {
                var result = await _usersCollection.DeleteOneAsync(user => user.Id == id);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID: {id}.");
                throw new Exception($"An error occurred while deleting user with ID: {id}.");
            }
        }
    }
}
