using MemoHubBackend.Data;
using MemoHubBackend.Dtos;
using MemoHubBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoHubBackend.Services
{
    public class TodoService
    {
        private readonly MemoHubDbContext _context;

        public TodoService(MemoHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoDto>> GetTodosByUserIdAsync(int userId)
        {
            return await _context.Todos
                .Where(t => t.Table.UserID == userId) // Ensure todos belong to the user
                .Include(t => t.Subnotes)
                .Include(t => t.Descriptions)
                .Select(t => new TodoDto
                {
                    TodoID = t.TodoID,
                    Title = t.Title,
                    Content = t.Content,
                    DueDate = t.DueDate,
                    Reminder = t.Reminder,
                    CreatedDate = t.CreatedDate,
                    ModifiedDate = t.ModifiedDate,
                    IsCompleted = t.IsCompleted,
                    TableID = t.TableID,
                    Subnotes = t.Subnotes.Select(s => new SubnoteDto
                    {
                        SubnoteID = s.SubnoteID,
                        TodoID = s.TodoID,
                        Content = s.Content,
                        CreatedDate = s.CreatedDate
                    }).ToList(),
                    Descriptions = t.Descriptions.Select(d => new DescriptionDto
                    {
                        DescriptionID = d.DescriptionID,
                        TodoID = d.TodoID,
                        Content = d.Content,
                        ReferencedWord = d.ReferencedWord,
                        CreatedDate = d.CreatedDate
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<TodoDto?> GetTodoByIdAsync(int id, int userId)
        {
            var todo = await _context.Todos
                .Include(t => t.Subnotes)
                .Include(t => t.Descriptions)
                .FirstOrDefaultAsync(t => t.TodoID == id && t.Table.UserID == userId); // Ensure todo belongs to the user

            if (todo == null) return null;

            return new TodoDto
            {
                TodoID = todo.TodoID,
                Title = todo.Title,
                Content = todo.Content,
                DueDate = todo.DueDate,
                Reminder = todo.Reminder,
                CreatedDate = todo.CreatedDate,
                ModifiedDate = todo.ModifiedDate,
                IsCompleted = todo.IsCompleted,
                TableID = todo.TableID,
                Subnotes = todo.Subnotes.Select(s => new SubnoteDto
                {
                    SubnoteID = s.SubnoteID,
                    TodoID = s.TodoID,
                    Content = s.Content,
                    CreatedDate = s.CreatedDate
                }).ToList(),
                Descriptions = todo.Descriptions.Select(d => new DescriptionDto
                {
                    DescriptionID = d.DescriptionID,
                    TodoID = d.TodoID,
                    Content = d.Content,
                    ReferencedWord = d.ReferencedWord,
                    CreatedDate = d.CreatedDate
                }).ToList()
            };
        }

        public async Task<TodoDto?> CreateTodoAsync(TodoDto todoDto, int userId)
        {
            // Ensure the table belongs to the user
            var tableExists = await _context.Tables.AnyAsync(t => t.TableID == todoDto.TableID && t.UserID == userId);
            if (!tableExists)
            {
                return null; // or throw a custom exception
            }

            var todo = new Todo
            {
                TableID = todoDto.TableID,
                Title = todoDto.Title,
                Content = todoDto.Content,
                DueDate = todoDto.DueDate,
                Reminder = todoDto.Reminder,
                CreatedDate = DateTime.UtcNow,
                IsCompleted = todoDto.IsCompleted
            };

            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return new TodoDto
            {
                TodoID = todo.TodoID,
                Title = todo.Title,
                Content = todo.Content,
                DueDate = todo.DueDate,
                Reminder = todo.Reminder,
                CreatedDate = todo.CreatedDate,
                ModifiedDate = todo.ModifiedDate,
                IsCompleted = todo.IsCompleted,
                TableID = todo.TableID
            };
        }

        public async Task<bool> UpdateTodoAsync(TodoDto todoDto, int userId)
        {
            var todo = await _context.Todos
        .Include(t => t.Table) // Include the related Table to verify ownership
        .FirstOrDefaultAsync(t => t.TodoID == todoDto.TodoID);

            if (todo == null) return false;

            if (!await _context.Tables.AnyAsync(t => t.TableID == todoDto.TableID))
            {
                // If TableID does not exist, return false or handle the error accordingly
                return false;
            }

            /*var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.TodoID == todoDto.TodoID && t.Table.UserID == userId); // Ensure todo belongs to the user
            */

            // Ensure that the TableID belongs to the same user
            var table = await _context.Tables
                .FirstOrDefaultAsync(t => t.TableID == todoDto.TableID && t.UserID == userId);

            if (table == null)
            {
                // If the provided TableID does not belong to the user, reject the update
                return false;
            }

            todo.Title = todoDto.Title;
            todo.Content = todoDto.Content;
            todo.DueDate = todoDto.DueDate;
            todo.Reminder = todoDto.Reminder;
            todo.ModifiedDate = DateTime.UtcNow;
            todo.IsCompleted = todoDto.IsCompleted;
            todo.TableID = todoDto.TableID;

            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTodoAsync(int todoId, int userId)
        {
            var todo = await _context.Todos
                .FirstOrDefaultAsync(t => t.TodoID == todoId && t.Table.UserID == userId); // Ensure todo belongs to the user

            if (todo == null) return false;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
