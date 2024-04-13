using Hotelservices.UserAuth.IdentityModels;
using HotelServices.Shared.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hotelservices.UserAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "ADMIN")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // User CRUD Operations
        [HttpGet("get-user/{userId}")]
        [AllowAnonymous] // Allow access without authorization
        public async Task<IActionResult> GetUser(string userId)
        {
            _logger.LogInformation($"Retrieving user with ID {userId}...");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
                return NotFound("User not found.");
            }
            return Ok(user);
        }

        [HttpGet("get-users")]
        public IActionResult GetUsers()
        {
            _logger.LogInformation("Retrieving users...");
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            _logger.LogInformation("Deleting user...");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
                return NotFound("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? Ok("User deleted successfully.") : BadRequest(result.Errors);
        }

        [HttpPost("modify-user-role/{userId}")]
        public async Task<IActionResult> ModifyUserRole(string userId, RoleType roleType)
        {
            _logger.LogInformation("Modifying user role...");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state.");
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {userId} not found.");
                return NotFound("User not found.");
            }

            var roleName = roleType.ToString();
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                _logger.LogWarning($"Role {roleName} does not exist.");
                return BadRequest("Role does not exist.");
            }

            var result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to remove existing roles from user.");
                return BadRequest(result.Errors);
            }

            result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded ? Ok("User role modified successfully.") : BadRequest(result.Errors);
        }

        // Role CRUD Operations

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] RoleType newRole)
        {
            _logger.LogInformation("Creating new role...");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state.");
                return BadRequest(ModelState);
            }

            var roleName = newRole.ToString();
            var role = new ApplicationRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded ? Ok("Role created successfully.") : BadRequest(result.Errors);
        }

        [HttpGet("get-roles")]
        public IActionResult GetRoles()
        {
            _logger.LogInformation("Retrieving roles...");
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            _logger.LogInformation("Deleting role...");
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                _logger.LogWarning($"Role with ID {roleId} not found.");
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded ? Ok("Role deleted successfully.") : BadRequest(result.Errors);
        }
    }
}
