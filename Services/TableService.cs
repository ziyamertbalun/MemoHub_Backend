using MemoHubBackend.Data;
using MemoHubBackend.Dtos;
using MemoHubBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoHubBackend.Services
{
    public class TableService
    {
        private readonly MemoHubDbContext _context;

        public TableService(MemoHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TableDto>> GetTablesByUserIdAsync(int userId)
        {
            return await _context.Tables
                .Where(t => t.UserID == userId)
                .Select(t => new TableDto
                {
                    TableID = t.TableID,
                    Name = t.Name,
                    UserID = t.UserID,
                    CreatedDate = t.CreatedDate,
                    ModifiedDate = t.ModifiedDate
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<TableDto>> GetTablesAsync()
        {
            return await _context.Tables
                .Select(t => new TableDto
                {
                    TableID = t.TableID,
                    Name = t.Name,
                    UserID = t.UserID,
                    CreatedDate = t.CreatedDate,
                    ModifiedDate = t.ModifiedDate
                })
                .ToListAsync();
        }

        public async Task<TableDto?> GetTableByIdAsync(int tableId)
        {
            var table = await _context.Tables
                .SingleOrDefaultAsync(t => t.TableID == tableId);

            if (table == null)
                return null;

            return new TableDto
            {
                TableID = table.TableID,
                Name = table.Name,
                UserID = table.UserID,
                CreatedDate = table.CreatedDate,
                ModifiedDate = table.ModifiedDate
            };
        }

        public async Task<TableDto?> CreateTableAsync(TableDto tableDto)
        {
            // Check if the user exists
            var userExists = await _context.Users.AnyAsync(u => u.UserID == tableDto.UserID);
            if (!userExists)
            {
                // User does not exist, return null or handle accordingly
                return null; // or throw a custom exception
            }

            var table = new Table
            {
                Name = tableDto.Name,
                UserID = tableDto.UserID,
                CreatedDate = DateTime.UtcNow
            };

            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            return new TableDto
            {
                TableID = table.TableID,
                Name = table.Name,
                UserID = table.UserID,
                CreatedDate = table.CreatedDate,
                ModifiedDate = table.ModifiedDate
            };
        }

        public async Task<bool> UpdateTableAsync(int tableId, TableDto tableDto)
        {
            var table = await _context.Tables.FindAsync(tableId);

            if (table == null)
                return false;

            table.Name = tableDto.Name;
            table.ModifiedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTableAsync(int tableId)
        {
            var table = await _context.Tables.FindAsync(tableId);

            if (table == null)
                return false;

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
