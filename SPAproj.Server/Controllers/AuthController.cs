using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPAproj.Server.Repo;
using SPAproj.Server.Models;

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
            var success = await _userManager.Login(loginDto.Username, loginDto.Password);

            if (!success)
            {
                return Unauthorized("Invalid username or password.");
            }

            // Generate token or handle post-login logic here
            return Ok(new { message = "Login successful" });
        }
    }
}
