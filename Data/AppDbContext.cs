using Microsoft.EntityFrameworkCore;
using ShopApp.Models;

namespace ShopApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "معالجات", Description = "CPUs", CreatedAt = new DateTime(2025, 1, 1) },
            new Category { Id = 2, Name = "كروت شاشة", Description = "GPUs", CreatedAt = new DateTime(2025, 1, 1) }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Ryzen 7 7800X3D", Price = 1750m, Stock = 12, IsAvailable = true, CategoryId = 1 },
            new Product { Id = 2, Name = "Intel Core i5-14600K", Price = 1250m, Stock = 8, IsAvailable = true, CategoryId = 1 },
            new Product { Id = 3, Name = "RTX 4070 Super", Price = 2600m, Stock = 5, IsAvailable = true, CategoryId = 2 }
        );
    }
}
