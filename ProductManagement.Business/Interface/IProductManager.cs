using ProductManagement.Business.Models;
using ProductManagement.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Business.Interface
{
    public interface IProductManager
    {
        Task<OperationResult<ProductBusinessModel>> GetById(int id);
        Task<OperationResult<IList<ProductBusinessModel>>> GetByNameAsync(string name);
        Task<OperationResult<IList<ProductBusinessModel>>> GetByStockLevelAsync(int min, int max);
        Task<OperationResult<StockUpdateEnum>> UpdateStockAsync(ProductBusinessModel model, StockOperationEnum operation);
        Task<OperationResult<IList<ProductBusinessModel>>> GetAll();
        Task<OperationResult<ProductBusinessModel>> EditProduct(ProductBusinessModel model);
        Task<OperationResult<ProductBusinessModel>> CreateProduct(ProductBusinessModel model);
        Task<OperationResult<ProductBusinessModel>> DeleteProduct(int id);
    }
}
