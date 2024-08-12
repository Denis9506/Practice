using Microsoft.EntityFrameworkCore;
using Practice.Models;

namespace Practice.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Products)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<Product> Products { get; set; }
    }
}
