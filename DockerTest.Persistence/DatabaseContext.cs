using DockerTest.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerTest.Persistence
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
    }
}
