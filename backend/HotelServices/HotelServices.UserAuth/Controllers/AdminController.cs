using Hotelservices.UserAuth.IdentityModels;
using HotelServices.Shared.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Hotelservices.UserAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // User CRUD Operations

        [HttpGet("get-users")]
        public IActionResult GetUsers() => Ok(_userManager.Users.ToList());

        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded ? Ok("User deleted successfully.") : BadRequest(result.Errors);
        }

        [HttpPost("modify-user-role/{userId}")]
        public async Task<IActionResult> ModifyUserRole(string userId, RoleType roleType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var roleName = roleType.ToString();
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return BadRequest("Role does not exist.");

            var result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded ? Ok("User role modified successfully.") : BadRequest(result.Errors);
        }

        // Role CRUD Operations

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole([FromBody] RoleType newRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var role = new ApplicationRole { Name = newRole.ToString() };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded ? Ok("Role created successfully.") : BadRequest(result.Errors);
        }

        [HttpGet("get-roles")]
        public IActionResult GetRoles() => Ok(_roleManager.Roles.ToList());

        [HttpDelete("delete-role/{roleId}")]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return NotFound("Role not found.");

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded ? Ok("Role deleted successfully.") : BadRequest(result.Errors);
        }
    }
}
