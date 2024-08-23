using MemoHubBackend.Data;
using MemoHubBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MemoHubBackend.Services
{
    public class UserService
    {
        private readonly MemoHubDbContext _context;

        public UserService(MemoHubDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"user with ID {userId} not found.");
            }
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with username {username} not found.");
            }
            return user;
        }
    }
}
