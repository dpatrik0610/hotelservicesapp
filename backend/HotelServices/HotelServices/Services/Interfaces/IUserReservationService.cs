using System.Collections.Generic;
using System.Threading.Tasks;
using HotelServices.Shared.Models;

namespace HotelServices.Services.Interfaces
{
    public interface IUserReservationService
    {
        Task<List<UserReservation>> GetAllUserReservationsAsync();
        Task<UserReservation> GetUserReservationByIdAsync(string id);
        Task<List<UserReservation>> GetUserReservationsByUserIdAsync(string userId);
        Task<List<UserReservation>> GetUserReservationsByRoomIdAsync(string roomId);
        Task CreateUserReservationAsync(UserReservation userReservation);
        Task<bool> UpdateUserReservationAsync(string id, UserReservation userReservation);
        Task<bool> DeleteUserReservationAsync(string id);
    }
}
