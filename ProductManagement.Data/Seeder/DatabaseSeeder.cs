using Microsoft.EntityFrameworkCore;
using ProductManagement.Data.Context;
using ProductManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProductManagement.Data.Seeder
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            var products = new[]
            {
            new Product
            {
                Name = "Product A",
                Description = "Sample product A for testing purposes.",
                Price = 10.00m,
                Stock = 100,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Product B",
                Description = "Sample product B for testing purposes.",
                Price = 20.00m,
                Stock = 50,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                Name = "Product C",
                Description = "Sample product C for testing purposes.",
                Price = 30.00m,
                Stock = 25,
                CreatedAt = DateTime.UtcNow
            }
        };

            var productNames = products
                .Select(p => p.Name)
                .ToList();

            var existingProductNames = await context.Products
                .Where(p => productNames.Contains(p.Name))
                .Select(p => p.Name)
                .ToListAsync();

            var productsToAdd = products
                .Where(p => !existingProductNames.Contains(p.Name))
                .ToList();

            if (productsToAdd.Count == 0)
                return;

            await context.Products.AddRangeAsync(productsToAdd);

            await context.SaveChangesAsync();
        }
    }
}
