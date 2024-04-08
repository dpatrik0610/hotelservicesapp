using HotelServices.Database;
using HotelServices.Services;
using HotelServices.Services.Interfaces;
using HotelServices.Shared.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace HotelServices.Tests
{
    public class RoomServiceTests
    {
        private readonly Mock<IMongoDatabaseProvider> _mockRepository;
        private readonly ILogger<RoomService> _logger;
        private readonly IRoomService _roomService;

        public RoomServiceTests()
        {
            _mockRepository = new Mock<IMongoDatabaseProvider>();
            _logger = new Mock<ILogger<RoomService>>().Object;
            _roomService = new RoomService(_mockRepository.Object, _logger);
        }

        [Theory]
        [InlineData(101)]
        [InlineData(102)]
        [InlineData(103)]
        public async Task GetRoomByNumberAsync_ExistingRoomNumber_ReturnsRoom(int roomNumber)
        {
            // Arrange
            var expectedRoom = new Room { RoomNumber = roomNumber };

            // Act
            var result = await _roomService.GetRoomByNumberAsync(roomNumber);

            // Assert
            Assert.Equal(expectedRoom.RoomNumber, result.RoomNumber);
        }

        [Theory]
        [InlineData(999)]
        [InlineData(888)]
        [InlineData(777)]
        public async Task GetRoomByNumberAsync_NonExistingRoomNumber_ReturnsNull(int roomNumber)
        {
            // Act
            var result = await _roomService.GetRoomByNumberAsync(roomNumber);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllRoomsAsync_WithFilledData_ReturnsListOfRooms()
        {
            // Arrange
            var data = new List<Room>()
            { 
                new Room() { RoomNumber = 101, Availability = true},
                new Room() { RoomNumber = 102, Availability = true},
                new Room() { RoomNumber = 103, Availability = false}
            };
            await _roomService.AddRoomsAsync(data);

            // Act
            var result = await _roomService.GetAllRoomsAsync();

            // Assert
            Assert.IsType<List<Room>>(result);
            Assert.NotEmpty(result);
            Assert.Equal(data, result);
        }

        [Fact]
        public async Task GetAvailableRoomsAsync_WhenRoomsAvailable_ReturnsListOfAvailableRooms()
        {
            // Arrange
            var data = new List<Room>()
            {
                new Room() { RoomNumber = 101, Availability = true},
                new Room() { RoomNumber = 102, Availability = true},
                new Room() { RoomNumber = 103, Availability = true}
            };

            // Act
            var result = await _roomService.GetAvailableRoomsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.All(result, room => Assert.True(room.Availability));
        }
        [Fact]
        public async Task GetAvailableRoomsAsync_WhenRoomsNotAvailable_ReturnsListOfAvailableRooms()
        {
            // Arrange
            var data = new List<Room>()
            {
                new Room() { RoomNumber = 101, Availability = false},
                new Room() { RoomNumber = 102, Availability = false},
                new Room() { RoomNumber = 103, Availability = false}
            };

            // Act
            var result = await _roomService.GetAvailableRoomsAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}
