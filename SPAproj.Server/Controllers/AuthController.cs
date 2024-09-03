using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SPAproj.Server.Repo;
using SPAproj.Server.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SPAproj.Models;
using System.Net.Http;

namespace SPAproj.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager _userManager;
        private readonly HttpClient _httpClient;
        private readonly ConfigurationService _configurationService;
        public AuthController(UserManager userManager, HttpClient httpClient, ConfigurationService configurationService) 
        {
            _userManager = userManager;
            _httpClient = httpClient;
            _configurationService = configurationService;
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
                return BadRequest("Invalid username or password.");
            }

            
            return Ok(new { message = "Login successful" });
        }
        [Authorize]
        [HttpGet("userInfo")]
        public IActionResult GetUserInfo()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not logged in.");
            }

            var username = HttpContext.User.Identity.Name;
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var roleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null)
            {
                return BadRequest("User ID not found.");
            }

            var userInfo = new
            {
                Username = username,
                UserId = userIdClaim.Value,
                Role = roleClaim.Value
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


        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { message = "Logout successful" });
        }

        
        [HttpGet("getData")]
        public IActionResult GetData()
        {
            
            return Ok("This is data only for admins!");
        }

        [Authorize(Policy ="AdminOnly")]
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UserStatusUpdateDto statusUpdate)
        {
            if (string.IsNullOrEmpty(statusUpdate.Username))
            {
                return BadRequest(new { message = "Username is required." });
            }

            bool updateResult = await _userManager.UpdateUserStatus(statusUpdate.Username, statusUpdate.NewStatus);
            if (updateResult)
            {
                return Ok(new { message = $"User status for {statusUpdate.Username} updated successfully to {statusUpdate.NewStatus}." });
            }
            else
            {
                return NotFound(new { message = $"Failed to update status for {statusUpdate.Username}. User not found or status record missing." });
            }
        }
        [HttpGet("getaccounts")]
        public async Task<ActionResult<List<Account>>> GetAccounts()
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_configurationService.GetParameterValue("accountapi"));

            response.EnsureSuccessStatusCode();

            var accounts = await response.Content.ReadFromJsonAsync<List<Account>>();

            return Ok(accounts);
        }

    }
}
