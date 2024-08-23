using MemoHubBackend.Data;
using MemoHubBackend.Dtos;
using MemoHubBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoHubBackend.Services
{
    public class SubnoteService
    {
        private readonly MemoHubDbContext _context;

        public SubnoteService(MemoHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubnoteDto>> GetSubnotesAsync(int userId)
        {
            return await _context.Subnotes
                .Include(s => s.Todo)
                    .ThenInclude(t => t.Table)
                .Where(s => s.Todo.Table.UserID == userId)  // Check UserID from Table
                .Select(s => new SubnoteDto
                {
                    SubnoteID = s.SubnoteID,
                    TodoID = s.TodoID,
                    Content = s.Content,
                    CreatedDate = s.CreatedDate
                })
                .ToListAsync();
        }

        public async Task<SubnoteDto?> GetSubnoteByIdAsync(int subnoteId, int userId)
        {
            var subnote = await _context.Subnotes
                .Include(s => s.Todo)
                    .ThenInclude(t => t.Table)
                .FirstOrDefaultAsync(s => s.SubnoteID == subnoteId && s.Todo.Table.UserID == userId);  // Check UserID from Table

            if (subnote == null) return null;

            return new SubnoteDto
            {
                SubnoteID = subnote.SubnoteID,
                TodoID = subnote.TodoID,
                Content = subnote.Content,
                CreatedDate = subnote.CreatedDate
            };
        }

        public async Task<bool> CreateSubnoteAsync(SubnoteDto subnoteDto, int userId)
        {
            var todo = await _context.Todos
                .Include(t => t.Table)
                .FirstOrDefaultAsync(t => t.TodoID == subnoteDto.TodoID && t.Table.UserID == userId);  // Check UserID from Table

            if (todo == null) return false;

            var subnote = new Subnote
            {
                TodoID = subnoteDto.TodoID,
                Content = subnoteDto.Content,
                CreatedDate = DateTime.UtcNow
            };

            _context.Subnotes.Add(subnote);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateSubnoteAsync(SubnoteDto subnoteDto, int userId)
        {
            var subnote = await _context.Subnotes
                .Include(s => s.Todo)
                    .ThenInclude(t => t.Table)
                .FirstOrDefaultAsync(s => s.SubnoteID == subnoteDto.SubnoteID && s.Todo.Table.UserID == userId);  // Check UserID from Table

            if (subnote == null) return false;

            subnote.Content = subnoteDto.Content;

            _context.Subnotes.Update(subnote);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSubnoteAsync(int subnoteId, int userId)
        {
            var subnote = await _context.Subnotes
                .Include(s => s.Todo)
                    .ThenInclude(t => t.Table)
                .FirstOrDefaultAsync(s => s.SubnoteID == subnoteId && s.Todo.Table.UserID == userId);  // Check UserID from Table

            if (subnote == null) return false;

            _context.Subnotes.Remove(subnote);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
