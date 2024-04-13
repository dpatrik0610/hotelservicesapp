using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HotelServices.Shared.Models.Enums;
using Hotelservices.UserAuth.Helpers;
using Hotelservices.UserAuth.Models;
using Hotelservices.UserAuth.IdentityModels;

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

        public AccountController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            JwtTokenGenerator jwtTokenGenerator
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = await _userManager.FindByNameAsync(model.Username);
            if (applicationUser == null)
            {
                ModelState.AddModelError(nameof(model.Username), "Invalid Username or Password");
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(applicationUser, model.Password, false, false);
            if (!result.Succeeded)
            {
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = new ApplicationUser
            {
                UserName = model.Username
            };

            var result = await _userManager.CreateAsync(applicationUser, model.Password);
            if (!result.Succeeded)
            {
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
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
