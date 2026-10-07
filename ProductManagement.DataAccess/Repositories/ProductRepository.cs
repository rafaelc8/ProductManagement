using Microsoft.EntityFrameworkCore;
using ProductManagement.Common.Enums;
using ProductManagement.Data.Context;
using ProductManagement.Data.Models;
using ProductManagement.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess.Repositories
{
    internal class ProductRepository : BaseRepository<Product, int>, IProductRepository
    {
        public ProductRepository(IDbContextFactory<ApplicationDbContext> dbContextFactory) : base(dbContextFactory)
        {
        }

        public async Task<List<Product>> GetByNameAsync(ApplicationDbContext context, string name)
        {
            return await context.Products
                .AsNoTracking()
                .Where(x => x.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<List<Product>> GetByStockLevelAsync(ApplicationDbContext context, int min, int max)
        {
            return await context.Products
                .AsNoTracking()
                .Where(x => x.Stock >= min && x.Stock <= max)
                .ToListAsync();
        }

        public async Task<bool> AddToStockAsync(ApplicationDbContext context, int id, int quantity)
        {
            var affectedRows = await context.Products
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Stock, x => x.Stock + quantity)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow));

            return affectedRows > 0;
        }

        public async Task<StockUpdateEnum> DecrementStockAsync(ApplicationDbContext context, int id, int quantity)
        {
            var affectedRows = await context.Products
                .Where(x => x.Id == id && x.Stock >= quantity)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.Stock, x => x.Stock - quantity)
                    .SetProperty(x => x.UpdatedAt, DateTime.UtcNow));

            if (affectedRows > 0)
                return StockUpdateEnum.Success;

            var exists = await context.Products.AnyAsync(x => x.Id == id);

            return exists ? StockUpdateEnum.InsufficientStock : StockUpdateEnum.ProductNotFound;
        }
    }
}
