using MemoHubBackend.Dtos;
using MemoHubBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemoHubBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubnoteController : ControllerBase
    {
        private readonly SubnoteService _subnoteService;

        public SubnoteController(SubnoteService subnoteService)
        {
            _subnoteService = subnoteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubnotes()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var subnotes = await _subnoteService.GetSubnotesAsync(int.TryParse(userIdClaim, out int userId) ? userId : 0);
            return Ok(subnotes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubnote([FromBody] SubnoteDto subnoteDto)
        {
            if (subnoteDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid subnote data.");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var createdSubnote = await _subnoteService.CreateSubnoteAsync(subnoteDto, userId);
            if (createdSubnote)
            {
                return Ok("Subnote created successfully.");
            }
            return BadRequest("Subnote creation failed.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubnoteById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var subnoteDto = await _subnoteService.GetSubnoteByIdAsync(id, userId);
            if (subnoteDto != null)
            {
                return Ok(subnoteDto);
            }
            return NotFound("Subnote not found.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubnote(int id, [FromBody] SubnoteDto subnoteDto)
        {
            if (subnoteDto == null || !ModelState.IsValid || id != subnoteDto.SubnoteID)
            {
                return BadRequest("Invalid subnote data.");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var updatedSubnote = await _subnoteService.UpdateSubnoteAsync(subnoteDto, userId);
            if (updatedSubnote)
            {
                return Ok("Subnote updated successfully.");
            }
            return BadRequest("Subnote update failed.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubnote(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var deletedSubnote = await _subnoteService.DeleteSubnoteAsync(id, userId);
            if (deletedSubnote)
            {
                return Ok("Subnote deleted successfully.");
            }
            return BadRequest("Subnote deletion failed.");
        }
    }
}
