using CelticsRestaurantAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CelticsRestaurantAPI.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
   
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customers Table Configuration
            builder.Entity<Customer>().ToTable("Customers");
            builder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Email).IsRequired().HasMaxLength(255);
                entity.Property(c => c.PhoneNumber).HasMaxLength(20);
            });

            // Admins Table Configuration
            builder.Entity<Admin>().ToTable("Admins");
            builder.Entity<Admin>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Name).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Email).IsRequired().HasMaxLength(255);
                entity.Property(a => a.PhoneNumber).HasMaxLength(20);
            });

            // FoodItems Table Configuration
            builder.Entity<FoodItem>().ToTable("FoodItems");
            builder.Entity<FoodItem>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Name).IsRequired().HasMaxLength(100);
                entity.Property(f => f.Price).IsRequired().HasPrecision(10, 2);
                entity.Property(f => f.Description).HasMaxLength(500);
                entity.Property(f => f.ImageUrl).HasMaxLength(255);

                // Relationship with OrderItems
                entity.HasMany(f => f.OrderItems)
                      .WithOne(oi => oi.FoodItem)
                      .HasForeignKey(oi => oi.FoodItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Orders Table Configuration
            builder.Entity<Order>().ToTable("Orders");
            builder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.PhoneNumber).HasMaxLength(20).IsRequired();
                entity.Property(o => o.Address).HasMaxLength(500).IsRequired();
                entity.Property(o => o.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(o => o.DatePlaced).IsRequired();
                entity.Property(o => o.Status).HasMaxLength(50);
                entity.Property(o => o.TotalPrice).HasPrecision(10, 2).IsRequired();

                // Foreign Key Relationship with Customer
                entity.HasOne(o => o.Customer)
                      .WithMany()
                      .HasForeignKey(o => o.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Relationship with OrderItems
                entity.HasMany(o => o.OrderItems)
                      .WithOne(oi => oi.Order)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItems Table Configuration
            builder.Entity<OrderItem>().ToTable("OrderItems");
            builder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);
                entity.Property(oi => oi.Quantity).IsRequired();

                // Foreign Key Relationships
                entity.HasOne(oi => oi.Order)
                      .WithMany(o => o.OrderItems)
                      .HasForeignKey(oi => oi.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(oi => oi.FoodItem)
                      .WithMany(f => f.OrderItems)
                      .HasForeignKey(oi => oi.FoodItemId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Default string column lengths
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetColumnType() == null)
                    {
                        property.SetColumnType("VARCHAR(255)");
                    }
                }
            }
        }
    }
}
