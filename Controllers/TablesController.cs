using MemoHubBackend.Dtos;
using MemoHubBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemoHubBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly TableService _tableService;

        public TableController(TableService tableService)
        {
            _tableService = tableService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] TableDto tableDto)
        {
            if (tableDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid table data.");
            }

            // Extract UserID from the JWT token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            // Convert the userId to int
            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid UserID.");
            }

            // Create TableDto with extracted UserID
            var tableDtoWithUserId = new TableDto
            {
                Name = tableDto.Name,
                UserID = userIdInt
            };

            var createdTable = await _tableService.CreateTableAsync(tableDtoWithUserId);

            if (createdTable != null)
            {
                return CreatedAtAction(nameof(GetTableById), new { id = createdTable.TableID }, createdTable);
            }

            return NotFound("User not found. Table creation failed.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(int id)
        {
            // Ensure the user is authenticated
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid UserID.");
            }

            var tableDto = await _tableService.GetTableByIdAsync(id);
            if (tableDto != null && tableDto.UserID == userIdInt)
            {
                return Ok(tableDto);
            }

            return NotFound("Table not found or access denied.");
        }

        [HttpGet]
        public async Task<IActionResult> GetTables()
        {
            // Ensure the user is authenticated
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid UserID.");
            }

            var tables = await _tableService.GetTablesByUserIdAsync(userIdInt);
            return Ok(tables);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(int id, [FromBody] TableDto tableDto)
        {
            if (tableDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid table data.");
            }

            // Ensure the user is authenticated
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid UserID.");
            }

            var tableDtoFromService = await _tableService.GetTableByIdAsync(id);
            if (tableDtoFromService == null || tableDtoFromService.UserID != userIdInt)
            {
                return NotFound("Table not found or access denied.");
            }

            var success = await _tableService.UpdateTableAsync(id, tableDto);
            if (success)
            {
                return NoContent(); // Successfully updated
            }

            return NotFound("Table not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            // Ensure the user is authenticated
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized("User not authenticated.");
            }

            if (!int.TryParse(userId, out int userIdInt))
            {
                return BadRequest("Invalid UserID.");
            }

            var tableDtoFromService = await _tableService.GetTableByIdAsync(id);
            if (tableDtoFromService == null || tableDtoFromService.UserID != userIdInt)
            {
                return NotFound("Table not found or access denied.");
            }

            var success = await _tableService.DeleteTableAsync(id);
            if (success)
            {
                return NoContent(); // Successfully deleted
            }

            return NotFound("Table not found.");
        }
    }
}
