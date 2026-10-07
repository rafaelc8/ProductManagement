using ProductManagement.Business.Interface;
using ProductManagement.Business.Models;
using ProductManagement.Common.Enums;
using ProductManagement.Data.Models;
using ProductManagement.DataAccess.Interfaces;

namespace ProductManagement.Business.Managers
{
    public class ProductManager : BaseManager, IProductManager
    {
        private readonly IProductRepository productRepository;

        public ProductManager(IProductRepository productRepository,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.productRepository = productRepository;
        }

        public async Task<OperationResult<ProductBusinessModel>> GetById(int id)
        {
            return await ExecuteOperationAsync(async (context) =>
            {
                var product = await productRepository.GetByIdAsync(context, id);

                if (product == null)
                    return Error<ProductBusinessModel>(ErrorEnum.NotFound);

                return Success(ProductBusinessModel.FromDataModel(product));
            });
        }

        public async Task<OperationResult<IList<ProductBusinessModel>>> GetByNameAsync(string name)
        {
            return await ExecuteOperationAsync(async (context) =>
            {
                List<ProductBusinessModel> products = (await productRepository.GetByNameAsync(context, name)).Select(ProductBusinessModel.FromDataModel).ToList();

                return Success((IList<ProductBusinessModel>)products);
            });
        }

        public async Task<OperationResult<IList<ProductBusinessModel>>> GetByStockLevelAsync(int min, int max)
        {
            return await ExecuteOperationAsync(async (context) =>
            {
                List<ProductBusinessModel> products = (await productRepository.GetByStockLevelAsync(context, min, max)).Select(ProductBusinessModel.FromDataModel).ToList();

                return Success((IList<ProductBusinessModel>)products);
            });
        }

        public async Task<OperationResult<StockUpdateEnum>> UpdateStockAsync(ProductBusinessModel model, StockOperationEnum operation)
        {
            return await ExecuteOperationAsync(async context =>
            {
                StockUpdateEnum result;

                switch (operation)
                {
                    case StockOperationEnum.Add:
                        var updated = await productRepository.AddToStockAsync(context, model.Id, model.Stock);
                        result = updated ? StockUpdateEnum.Success : StockUpdateEnum.ProductNotFound;
                        break;

                    case StockOperationEnum.Decrement:
                        result = await productRepository.DecrementStockAsync(context, model.Id, model.Stock);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(operation));
                }

                if (result == StockUpdateEnum.ProductNotFound)
                    return Error<StockUpdateEnum>(ErrorEnum.NotFound);

                if (result == StockUpdateEnum.InsufficientStock)
                    return Error<StockUpdateEnum>(ErrorEnum.InsufficientStock);

                return Success(result);
            });
        }

        public async Task<OperationResult<ProductBusinessModel>> CreateProduct(ProductBusinessModel model)
        {
            return await ExecuteOperationAsync(async (context) =>
            {

                var entity = ProductBusinessModel.ToDataModel(model);

                await productRepository.InsertAsync(context, entity);
                await context.SaveChangesAsync();

                return Success(ProductBusinessModel.FromDataModel(entity));
            });
        }

        public async Task<OperationResult<ProductBusinessModel>> EditProduct(ProductBusinessModel model)
        {
            return await ExecuteOperationAsync(async (context) =>
            {
                Product dbProduct = await productRepository.GetByIdAsync(context, model.Id);
                if (dbProduct == null)
                    return Error<ProductBusinessModel>(ErrorEnum.NotFound);

                dbProduct.Stock = model.Stock;
                dbProduct.Name = model.Name;
                dbProduct.Description = model.Description;
                dbProduct.Price = model.Price;
                dbProduct.UpdatedAt = DateTime.UtcNow;

                await productRepository.UpdateAsync(context, dbProduct);
                await context.SaveChangesAsync();

                return Success(ProductBusinessModel.FromDataModel(dbProduct));
            });
        }

        public async Task<OperationResult<IList<ProductBusinessModel>>> GetAll()
        {
            return await ExecuteOperationAsync(async context =>
            {
                var products = (await productRepository.GetAllAsync(context))
                    .Select(ProductBusinessModel.FromDataModel)
                    .ToList();

                return Success((IList<ProductBusinessModel>)products);
            });
        }

        public async Task<OperationResult<ProductBusinessModel>> DeleteProduct(int id)
        {
            return await ExecuteOperationAsync(async (context) =>
            {
                Product dbProduct = await productRepository.GetByIdAsync(context, id);
                if (dbProduct == null)
                    return Error<ProductBusinessModel>(ErrorEnum.NotFound);


                await productRepository.DeleteAsync(context, dbProduct);
                await context.SaveChangesAsync();

                return Success(ProductBusinessModel.FromDataModel(dbProduct));
            });
        }
    }
}
