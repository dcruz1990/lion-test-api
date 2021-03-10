using TestWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TestWebApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(new User
            {
                Id = new System.Guid(),
                Username = "Administrator",
                LastName = "Last Name",
                Password = "FakePassword*!",
                Role = "Administrator"
            });

            builder.Entity<User>().HasData(new User
            {
                Id = new System.Guid(),
                Username = "Seller",
                LastName = "Last Name",
                Password = "FakePassword*!",
                Role = "Seller"
            });

            builder.Entity<User>().HasData(new User
            {
                Id = new System.Guid(),
                Username = "Client",
                LastName = "Last Name",
                Password = "FakePassword*!",
                Role = "Client"
            });
        }


    }
}