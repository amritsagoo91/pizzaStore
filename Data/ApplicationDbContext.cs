using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaStore.Models;

namespace PizzaStore.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Pizza> Pizzas { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial pizza data
        modelBuilder.Entity<Pizza>().HasData(
            new Pizza { Id = 1, Name = "Margherita", Price = 10.99m, Size = "Medium"},
            new Pizza { Id = 2, Name = "Pepperoni", Price = 12.99m, Size = "Large"},
            new Pizza { Id = 3, Name = "Veggie Delight", Price = 11.50m, Size = "Medium"},
            new Pizza { Id = 4, Name = "BBQ Chicken", Price = 13.99m, Size = "Large"},
            new Pizza { Id = 5, Name = "Hawaiian", Price = 12.50m, Size = "Medium"},
            new Pizza { Id = 6, Name = "Four Cheese", Price = 14.00m, Size = "Medium"},
            new Pizza { Id = 7, Name = "Buffalo Chicken", Price = 15.50m, Size = "Large"},
            new Pizza { Id = 8, Name = "Spinach & Feta", Price = 13.25m, Size = "Medium"},
            new Pizza { Id = 9, Name = "Meat Lovers", Price = 16.75m, Size = "Large"},
            new Pizza { Id = 10, Name = "Mushroom & Onion", Price = 12.99m, Size = "Medium"}
        );
    }

}
