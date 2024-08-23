using MemoHubBackend.Dtos;
using MemoHubBackend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MemoHubBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(userIdClaim, out int userId) ? userId : 0;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync([FromBody] TodoDto todoDto)
        {
            if (todoDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid todo data.");
            }

            var userId = GetUserIdFromToken();
            var createdTodo = await _todoService.CreateTodoAsync(todoDto, userId);

            if (createdTodo != null)
            {
                return CreatedAtAction(nameof(GetTodoById), new { id = createdTodo.TodoID }, createdTodo);
            }
            return BadRequest("Todo creation failed.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoById(int id)
        {
            var userId = GetUserIdFromToken();
            var todoDto = await _todoService.GetTodoByIdAsync(id, userId);

            if (todoDto != null)
            {
                return Ok(todoDto);
            }
            return NotFound("Todo not found.");
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var userId = GetUserIdFromToken();
            var todos = await _todoService.GetTodosByUserIdAsync(userId);
            return Ok(todos);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoAsync(int id, [FromBody] TodoDto todoDto)
        {
            if (todoDto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid todo data.");
            }

            todoDto.TodoID = id;
            var userId = GetUserIdFromToken();
            var updated = await _todoService.UpdateTodoAsync(todoDto, userId);

            if (updated)
            {
                return NoContent();
            }
            return NotFound("Todo not found.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoAsync(int id)
        {
            var userId = GetUserIdFromToken();
            var deleted = await _todoService.DeleteTodoAsync(id, userId);

            if (deleted)
            {
                return NoContent();
            }
            return NotFound("Todo not found.");
        }
    }
}
