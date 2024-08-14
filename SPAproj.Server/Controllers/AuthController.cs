using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPAproj.Server.Repo;
using SPAproj.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace SPAproj.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager _userManager;

        public AuthController(UserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (string.IsNullOrEmpty(registerDto.Username) || string.IsNullOrEmpty(registerDto.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var success = await _userManager.Register(registerDto.Username, registerDto.Password, registerDto.Role, 1);

            if (!success)
            {
                return BadRequest("Username is already taken.");
            }

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var success = await _userManager.Login(loginDto.Username, loginDto.Password,HttpContext);

            if (!success)
            {
                return Unauthorized("Invalid username or password.");
            }

            
            return Ok(new { message = "Login successful" });
        }

        [HttpGet("userInfo")]
        public IActionResult GetUserInfo()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not logged in.");
            }

            var username = HttpContext.User.Identity.Name;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return BadRequest("User ID not found.");
            }

            var userInfo = new
            {
                Username = username,
                UserId = userIdClaim.Value
            };

            return Ok(userInfo);
        }

        [HttpGet("isLoggedIn")]
        public IActionResult IsLoggedIn()
        {
            var isLoggedIn = _userManager.IsUserLoggedIn(HttpContext);

            if (!isLoggedIn)
            {
                return Unauthorized("User is not logged in.");
            }

            return Ok(new { message = "User is logged in" });
        }



        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout successful" });
        }

    }
}
