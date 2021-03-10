using TestWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;

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
                Id = 1,
                Username = "Administrator",
                LastName = "Last Name",
                Password = "FakePassword*!",
                Role = "Administrator"
            });

            builder.Entity<User>().HasData(new User
            {
                Id = 2,
                Username = "Seller",
                LastName = "Last Name",
                Password = "FakePassword*!",
                Role = "Seller"
            });

            builder.Entity<User>().HasData(new User
            {
                Id = 3,
                Username = "Client",
                LastName = "Last Name",
                Password = "FakePassword*!",
                Role = "Client"
            });
        }


    }
}