using MemoHubBackend.Dtos;
using MemoHubBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MemoHubBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUserByUsername(string username)
        {
            var userDto = await _userService.GetUserByUsernameAsync(username);
            if (userDto != null)
            {
                return Ok(userDto);
            }
            return NotFound("User not found.");
        }
    }
}
