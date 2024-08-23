using Microsoft.EntityFrameworkCore;
using MemoHubBackend.Models;

namespace MemoHubBackend.Data
{
    public class MemoHubDbContext : DbContext
    {
        public MemoHubDbContext(DbContextOptions<MemoHubDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } 
        public DbSet<Table> Tables { get; set; }
        public DbSet<Todo> Todos { get; set; }
        public DbSet<Subnote> Subnotes { get; set; }
        public DbSet<Description> Descriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) => base.OnModelCreating(modelBuilder);//
    }
}
