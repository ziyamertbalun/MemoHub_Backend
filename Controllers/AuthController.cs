using MemoHubBackend.Dtos;
using MemoHubBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MemoHubBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (registerDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid user data.");
            }

            var result = await _authService.RegisterAsync(registerDto);
            if (result)
            {
                return Ok("User registered successfully.");
            }
            return BadRequest("User registration failed.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (loginDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid login data.");
            }

            var token = await _authService.LoginAsync(loginDto);
            if (token != null)
            {
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid credentials.");
        }
    }
}
