using HotelServices.Shared.Models;
using HotelServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HotelServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserReservationController : ControllerBase
    {
        private readonly IUserReservationService _userReservationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UserReservationController> _logger;

        public UserReservationController(IUserReservationService userReservationService, IHttpContextAccessor httpContextAccessor, ILogger<UserReservationController> logger)
        {
            _userReservationService = userReservationService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateUserReservation(UserReservation userReservation)
        {
            try
            {
                await _userReservationService.CreateUserReservationAsync(userReservation);
                return Ok("User reservation created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user reservation.");
                return StatusCode(500, "An error occurred while creating user reservation.");
            }
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUserReservation(string id)
        {
            try
            {
                // Retrieve the user ID of the currently logged-in user
                var userId = _httpContextAccessor.HttpContext.User.FindFirst("sub")?.Value;
                if (userId == null)
                {
                    // User ID not found in claims
                    return Unauthorized();
                }

                // Check if the reservation belongs to the current user
                var userReservation = await _userReservationService.GetUserReservationByIdAsync(id);
                if (userReservation == null)
                {
                    // Reservation not found
                    return NotFound();
                }

                // Only allow deletion if the current user owns the reservation
                if (userReservation.UserId != userId)
                {
                    return Forbid();
                }

                // Delete the reservation
                var deleted = await _userReservationService.DeleteUserReservationAsync(id);
                if (!deleted)
                {
                    // Failed to delete the reservation
                    return StatusCode(500, $"An error occurred while deleting user reservation with ID: {id}");
                }

                // Reservation deleted successfully
                return Ok("User reservation deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user reservation with ID: {id}");
                return StatusCode(500, $"An error occurred while deleting user reservation with ID: {id}");
            }
        }
    }
}
