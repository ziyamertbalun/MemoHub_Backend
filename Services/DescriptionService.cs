using MemoHubBackend.Data;
using MemoHubBackend.Dtos;
using MemoHubBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoHubBackend.Services
{
    public class DescriptionService
    {
        private readonly MemoHubDbContext _context;

        public DescriptionService(MemoHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DescriptionDto>> GetDescriptionsByTodoIdAsync(int todoId)
        {
            return await _context.Descriptions
                .Where(d => d.TodoID == todoId)
                .Select(d => new DescriptionDto
                {
                    DescriptionID = d.DescriptionID,
                    TodoID = d.TodoID,
                    Content = d.Content,
                    ReferencedWord = d.ReferencedWord,
                    CreatedDate = d.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<DescriptionDto?> GetDescriptionByIdAsync(int descriptionId, string userId)
        {
            var description = await _context.Descriptions
                .Include(d => d.Todo)
                .FirstOrDefaultAsync(d => d.DescriptionID == descriptionId && d.Todo.Table.UserID == Convert.ToInt32(userId));

            if (description == null) return null;

            return new DescriptionDto
            {
                DescriptionID = description.DescriptionID,
                TodoID = description.TodoID,
                Content = description.Content,
                ReferencedWord = description.ReferencedWord,
                CreatedDate = description.CreatedDate
            };
        }

        public async Task<bool> CreateDescriptionAsync(DescriptionDto descriptionDto)
        {
            var description = new Description
            {
                TodoID = descriptionDto.TodoID,
                Content = descriptionDto.Content,
                ReferencedWord = descriptionDto.ReferencedWord,
                CreatedDate = DateTime.UtcNow
            };

            _context.Descriptions.Add(description);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateDescriptionAsync(DescriptionDto descriptionDto)
        {
            var description = await _context.Descriptions.FindAsync(descriptionDto.DescriptionID);

            if (description == null) return false;

            description.Content = descriptionDto.Content;
            description.ReferencedWord = descriptionDto.ReferencedWord;

            _context.Descriptions.Update(description);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteDescriptionAsync(int descriptionId)
        {
            var description = await _context.Descriptions.FindAsync(descriptionId);

            if (description == null) return false;

            _context.Descriptions.Remove(description);
            await _context.SaveChangesAsync();

            return true;
        }

        // Checks if a Description belongs to the user
        public async Task<bool> IsDescriptionOwnedByUser(int descriptionId, string userId)
        {
            return await _context.Descriptions
                .Include(d => d.Todo)
                .AnyAsync(d => d.DescriptionID == descriptionId && d.Todo.Table.UserID == Convert.ToInt32(userId));
        }

        // Checks if a Todo belongs to the user
        public async Task<bool> IsTodoOwnedByUser(int todoId, string userId)
        {
            return await _context.Todos
                .AnyAsync(t => t.TodoID == todoId && t.Table.UserID == Convert.ToInt32(userId));
        }
    }
}
