using HotelServices.Models;

namespace HotelServices.Services.Interfaces
{
    public interface IRoomService
    {
        Task<Room> GetRoomByNumberAsync(int roomNumber);
        Task<List<Room>> GetRoomsByNumbersAsync(List<int> roomNumbers);
        Task<List<Room>> GetAllRoomsAsync();
        Task<List<Room>> GetAvailableRoomsAsync();
        Task AddRoomAsync(Room room);
        Task AddRoomsAsync(List<Room> rooms);
        Task<bool> UpdateRoomAsync(int roomNumber, Room room);
        Task<bool> DeleteRoomAsync(int roomNumber);
    }
}
