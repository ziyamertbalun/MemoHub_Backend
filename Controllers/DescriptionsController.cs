using MemoHubBackend.Dtos;
using MemoHubBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MemoHubBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Enforces authentication
    public class DescriptionController : ControllerBase
    {
        private readonly DescriptionService _descriptionService;

        public DescriptionController(DescriptionService descriptionService)
        {
            _descriptionService = descriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDescription([FromBody] DescriptionDto descriptionDto)
        {
            if (descriptionDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid description data.");
            }

            // Extract user ID from JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid user.");
            }

            // Check if the Todo belongs to the user
            if (!await _descriptionService.IsTodoOwnedByUser(descriptionDto.TodoID, userId))
            {
                return Forbid("You do not have permission to add descriptions to this Todo.");
            }

            var createdDescription = await _descriptionService.CreateDescriptionAsync(descriptionDto);
            if (createdDescription)
            {
                return Ok("Description created successfully.");
            }
            return BadRequest("Description creation failed.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDescriptionById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid user.");
            }

            var descriptionDto = await _descriptionService.GetDescriptionByIdAsync(id, userId);
            if (descriptionDto != null)
            {
                return Ok(descriptionDto);
            }
            return NotFound("Description not found.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] DescriptionDto descriptionDto)
        {
            if (descriptionDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid description data.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid user.");
            }

            // Ensure the Description belongs to the user
            if (!await _descriptionService.IsDescriptionOwnedByUser(id, userId))
            {
                return Forbid("You do not have permission to update this Description.");
            }

            // Set the DescriptionID in the DTO to match the route parameter
            descriptionDto.DescriptionID = id;

            var updated = await _descriptionService.UpdateDescriptionAsync(descriptionDto);
            if (updated)
            {
                return Ok("Description updated successfully.");
            }
            return BadRequest("Description update failed.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDescription(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid user.");
            }

            // Ensure the Description belongs to the user
            if (!await _descriptionService.IsDescriptionOwnedByUser(id, userId))
            {
                return Forbid("You do not have permission to delete this Description.");
            }

            var deleted = await _descriptionService.DeleteDescriptionAsync(id);
            if (deleted)
            {
                return Ok("Description deleted successfully.");
            }
            return BadRequest("Description deletion failed.");
        }

        [HttpGet("todos/{todoId}")]
        public async Task<IActionResult> GetDescriptionsForTodo(int todoId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("Invalid user.");
            }

            if (!await _descriptionService.IsTodoOwnedByUser(todoId, userId))
            {
                return Forbid("You do not have permission to view descriptions for this Todo.");
            }

            var descriptions = await _descriptionService.GetDescriptionsByTodoIdAsync(todoId);
            return Ok(descriptions);
        }
    }
}
