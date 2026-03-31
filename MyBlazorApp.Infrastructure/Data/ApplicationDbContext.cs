using Microsoft.EntityFrameworkCore;
using MyBlazorApp.Domain.Entities;

namespace MyBlazorApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Seed initial data for Products
        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Laptop", Price = 1500 },
            new Product { Id = 2, Name = "Phone", Price = 800 },
            new Product { Id = 3, Name = "Tablet", Price = 500 }
        );

        // Seed initial data for Users (passwords will be hashed properly later)
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@example.com", PasswordHash = "hashed_password_here" },
            new User { Id = 2, Username = "user", Email = "user@example.com", PasswordHash = "hashed_password_here" }
        );
    }
}