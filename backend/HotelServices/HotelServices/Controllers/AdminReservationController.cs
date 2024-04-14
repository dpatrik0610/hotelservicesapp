using HotelServices.Controllers;
using HotelServices.Services.Interfaces;
using HotelServices.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelServices.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class AdminReservationController : Controller
    {
        private readonly IUserReservationService _userReservationService;
        private readonly ILogger<AdminReservationController> _logger;

        public AdminReservationController(IUserReservationService userReservationService, ILogger<AdminReservationController> logger)
        {
            _userReservationService = userReservationService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserReservations()
        {
            try
            {
                var userReservations = await _userReservationService.GetAllUserReservationsAsync();
                return Ok(userReservations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching user reservations.");
                return StatusCode(500, "An error occurred while fetching user reservations.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserReservationById(string id)
        {
            try
            {
                var userReservation = await _userReservationService.GetUserReservationByIdAsync(id);
                if (userReservation == null)
                    return NotFound();

                return Ok(userReservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching user reservation with ID: {id}");
                return StatusCode(500, $"An error occurred while fetching user reservation with ID: {id}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserReservation(string id, UserReservation userReservation)
        {
            try
            {
                var updated = await _userReservationService.UpdateUserReservationAsync(id, userReservation);
                if (!updated)
                    return NotFound();

                return Ok("User reservation updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user reservation with ID: {id}");
                return StatusCode(500, $"An error occurred while updating user reservation with ID: {id}");
            }
        }
    }
}
