using ProductManagement.Common.Enums;
using ProductManagement.Data.Context;
using ProductManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DataAccess.Interfaces
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<List<Product>> GetByNameAsync(ApplicationDbContext context, string name);
        Task<List<Product>> GetByStockLevelAsync(ApplicationDbContext context, int min, int max);
        Task<bool> AddToStockAsync(ApplicationDbContext context, int id, int quantity);
        Task<StockUpdateEnum> DecrementStockAsync(ApplicationDbContext context, int id, int quantity);
    }
}
