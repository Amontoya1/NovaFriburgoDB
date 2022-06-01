using Microsoft.EntityFrameworkCore;
using NovaFriburgoDB.Models.DataModels;

namespace NovaFriburgoDB.DataAccess
{
    public class NovaFriburgoDBContext : DbContext
    {
        public NovaFriburgoDBContext(DbContextOptions<NovaFriburgoDBContext> options) : base(options)
        {
          
        }

        // Add DbSets (Tables of our Data base)
        public DbSet<User>? Users { get; set; }
        public DbSet<Course>? Courses { get; set; }
        public DbSet<Chapter>? Chapters { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Student>? Students { get; set; }
    }
}
