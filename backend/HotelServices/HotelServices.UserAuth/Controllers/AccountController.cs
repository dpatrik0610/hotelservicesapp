using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HotelServices.Shared.Models.Enums;
using Hotelservices.UserAuth.Helpers;
using Hotelservices.UserAuth.Models;
using Hotelservices.UserAuth.IdentityModels;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotelservices.UserAuth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenGenerator jwtTokenGenerator,
            ILogger<AccountController> logger
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("User login attempt.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state.");
                return BadRequest(ModelState);
            }

            var applicationUser = await _userManager.FindByNameAsync(model.Username);
            if (applicationUser == null)
            {
                _logger.LogWarning($"User {model.Username} not found.");
                ModelState.AddModelError(nameof(model.Username), "Invalid Username or Password");
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(applicationUser, model.Password, false, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"User {model.Username} login failed.");
                ModelState.AddModelError("", "Invalid Username or Password");
                return BadRequest(ModelState);
            }

            // Retrieve user roles
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var token = _jwtTokenGenerator.GenerateJwtToken(applicationUser.Id.ToString(), applicationUser.UserName, roles.ToList());
            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            _logger.LogInformation("User registration attempt.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state.");
                return BadRequest(ModelState);
            }

            var applicationUser = new ApplicationUser
            {
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (!result.Succeeded)
            {
                _logger.LogWarning("User registration failed.");
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return BadRequest(ModelState);
            }

            // Add the user to the default role
            var addToRoleResult = await _userManager.AddToRoleAsync(applicationUser, RoleType.None.ToString());
            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError("Failed to assign role to the user.");
                ModelState.AddModelError("", "Failed to assign role to the user.");
                return BadRequest(ModelState);
            }

            var token = _jwtTokenGenerator.GenerateJwtToken(applicationUser.Id.ToString(), applicationUser.UserName, new List<string>() { "None" });
            return Ok(new { Token = token });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation("User logout.");

            var user = HttpContext.User.Identity.Name;

            if (!User.Identity.IsAuthenticated) return BadRequest(new { Message = "User is not logged in." });

            // User is authenticated, proceed with logout
            _logger.LogInformation($"User {user} logged out.");
            await _signInManager.SignOutAsync();
            var response = new { Message = "User logged out." };

            return Ok(response);
        }
    }
}
