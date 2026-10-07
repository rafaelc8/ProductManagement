using Moq;
using ProductManagement.Business.Managers;
using ProductManagement.Business.Models;
using ProductManagement.Common.Enums;
using ProductManagement.Data.Context;
using ProductManagement.Data.Models;
using ProductManagement.DataAccess.Interfaces;
using Xunit;

namespace ProductManagement.Tests
{
    public class ProductManagerTests
    {
        private readonly Mock<IProductRepository> productRepositoryMock;
        private readonly ProductManager productManager;

        public ProductManagerTests()
        {
            productRepositoryMock = new Mock<IProductRepository>();

            var serviceProvider = TestServiceProviderFactory.Create();

            productManager = new ProductManager(productRepositoryMock.Object, serviceProvider);
        }

        [Fact]
        public async Task GetById_WhenProductExists_ReturnsProduct()
        {
            var product = CreateProduct();

            productRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<ApplicationDbContext>(), product.Id))
                .ReturnsAsync(product);

            var result = await productManager.GetById(product.Id);

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(product.Id, result.Data.Id);
            Assert.Equal(product.Name, result.Data.Name);
            Assert.Equal(product.Description, result.Data.Description);
            Assert.Equal(product.Price, result.Data.Price);
            Assert.Equal(product.Stock, result.Data.Stock);
            Assert.Equal(product.CreatedAt, result.Data.CreatedAt);
        }

        [Fact]
        public async Task GetById_WhenProductDoesNotExist_ReturnsNotFound()
        {
            productRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<ApplicationDbContext>(), 999999))
                .ReturnsAsync((Product?)null);

            var result = await productManager.GetById(999999);

            Assert.False(result.Succeeded);
            Assert.Equal(ErrorEnum.NotFound, result.Error);
        }

        [Fact]
        public async Task GetByName_WhenProductsExist_ReturnsProducts()
        {
            var products = new List<Product>
            {
                CreateProduct(100001, "Product A"),
                CreateProduct(100002, "Product AB")
            };

            productRepositoryMock
                .Setup(x => x.GetByNameAsync(It.IsAny<ApplicationDbContext>(), "Product"))
                .ReturnsAsync(products);

            var result = await productManager.GetByNameAsync("Product");

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(100001, result.Data[0].Id);
            Assert.Equal("Product A", result.Data[0].Name);
            Assert.Equal(100002, result.Data[1].Id);
            Assert.Equal("Product AB", result.Data[1].Name);
        }

        [Fact]
        public async Task GetByName_WhenNoProductsExist_ReturnsEmptyList()
        {
            productRepositoryMock
                .Setup(x => x.GetByNameAsync(It.IsAny<ApplicationDbContext>(), "Unknown"))
                .ReturnsAsync(new List<Product>());

            var result = await productManager.GetByNameAsync("Unknown");

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task GetByStockLevel_WhenProductsExist_ReturnsProducts()
        {
            var products = new List<Product>
            {
                CreateProduct(100001, "Product A", 10),
                CreateProduct(100002, "Product B", 20)
            };

            productRepositoryMock
                .Setup(x => x.GetByStockLevelAsync(It.IsAny<ApplicationDbContext>(), 10, 20))
                .ReturnsAsync(products);

            var result = await productManager.GetByStockLevelAsync(10, 20);

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(2, result.Data.Count);
            Assert.Equal(10, result.Data[0].Stock);
            Assert.Equal(20, result.Data[1].Stock);
        }

        [Fact]
        public async Task GetByStockLevel_WhenNoProductsExist_ReturnsEmptyList()
        {
            productRepositoryMock
                .Setup(x => x.GetByStockLevelAsync(It.IsAny<ApplicationDbContext>(), 10, 20))
                .ReturnsAsync(new List<Product>());

            var result = await productManager.GetByStockLevelAsync(10, 20);

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }

        [Fact]
        public async Task AddStock_WhenProductExists_ReturnsSuccess()
        {
            productRepositoryMock
                .Setup(x => x.AddToStockAsync(It.IsAny<ApplicationDbContext>(), 100001, 10))
                .ReturnsAsync(true);

            var model = new ProductBusinessModel
            {
                Id = 100001,
                Stock = 10
            };

            var result = await productManager.UpdateStockAsync(model, StockOperationEnum.Add);

            Assert.True(result.Succeeded);
            Assert.Equal(StockUpdateEnum.Success, result.Data);

            productRepositoryMock.Verify(x =>
                x.AddToStockAsync(It.IsAny<ApplicationDbContext>(), 100001, 10), Times.Once);
        }

        [Fact]
        public async Task AddStock_WhenProductDoesNotExist_ReturnsNotFound()
        {
            productRepositoryMock
                .Setup(x => x.AddToStockAsync(It.IsAny<ApplicationDbContext>(), 999999, 10))
                .ReturnsAsync(false);

            var model = new ProductBusinessModel
            {
                Id = 999999,
                Stock = 10
            };

            var result = await productManager.UpdateStockAsync(model, StockOperationEnum.Add);

            Assert.False(result.Succeeded);
            Assert.Equal(ErrorEnum.NotFound, result.Error);
        }

        [Fact]
        public async Task DecrementStock_WhenStockIsAvailable_ReturnsSuccess()
        {
            productRepositoryMock
                .Setup(x => x.DecrementStockAsync(It.IsAny<ApplicationDbContext>(), 100001, 10))
                .ReturnsAsync(StockUpdateEnum.Success);

            var model = new ProductBusinessModel
            {
                Id = 100001,
                Stock = 10
            };

            var result = await productManager.UpdateStockAsync(model, StockOperationEnum.Decrement);

            Assert.True(result.Succeeded);
            Assert.Equal(StockUpdateEnum.Success, result.Data);

            productRepositoryMock.Verify(x =>
                x.DecrementStockAsync(It.IsAny<ApplicationDbContext>(), 100001, 10), Times.Once);
        }

        [Fact]
        public async Task DecrementStock_WhenStockIsInsufficient_ReturnsInsufficientStock()
        {
            productRepositoryMock
                .Setup(x => x.DecrementStockAsync(It.IsAny<ApplicationDbContext>(), 100001, 200))
                .ReturnsAsync(StockUpdateEnum.InsufficientStock);

            var model = new ProductBusinessModel
            {
                Id = 100001,
                Stock = 200
            };

            var result = await productManager.UpdateStockAsync(model, StockOperationEnum.Decrement);

            Assert.False(result.Succeeded);
            Assert.Equal(ErrorEnum.InsufficientStock, result.Error);
        }

        [Fact]
        public async Task DecrementStock_WhenProductDoesNotExist_ReturnsNotFound()
        {
            productRepositoryMock
                .Setup(x => x.DecrementStockAsync(It.IsAny<ApplicationDbContext>(), 999999, 10))
                .ReturnsAsync(StockUpdateEnum.ProductNotFound);

            var model = new ProductBusinessModel
            {
                Id = 999999,
                Stock = 10
            };

            var result = await productManager.UpdateStockAsync(model, StockOperationEnum.Decrement);

            Assert.False(result.Succeeded);
            Assert.Equal(ErrorEnum.NotFound, result.Error);
        }

        [Fact]
        public async Task EditProduct_WhenProductExists_UpdatesProduct()
        {
            var product = CreateProduct();

            productRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<ApplicationDbContext>(), product.Id))
                .ReturnsAsync(product);

            productRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<ApplicationDbContext>(), It.IsAny<Product>()))
                .ReturnsAsync((ApplicationDbContext context, Product product) => product);

            var model = new ProductBusinessModel
            {
                Id = product.Id,
                Name = "Updated Product",
                Description = "Updated description",
                Price = 25m,
                Stock = 75
            };

            var result = await productManager.EditProduct(model);

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(product.Id, result.Data.Id);
            Assert.Equal("Updated Product", result.Data.Name);
            Assert.Equal("Updated description", result.Data.Description);
            Assert.Equal(25m, result.Data.Price);
            Assert.Equal(75, result.Data.Stock);
            Assert.NotNull(result.Data.UpdatedAt);

            productRepositoryMock.Verify(x => x.UpdateAsync(
                It.IsAny<ApplicationDbContext>(),
                It.Is<Product>(p =>
                    p.Id == product.Id &&
                    p.Name == "Updated Product" &&
                    p.Description == "Updated description" &&
                    p.Price == 25m &&
                    p.Stock == 75)), Times.Once);
        }

        [Fact]
        public async Task EditProduct_WhenProductDoesNotExist_ReturnsNotFound()
        {
            productRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<ApplicationDbContext>(), 999999))
                .ReturnsAsync((Product?)null);

            var model = new ProductBusinessModel
            {
                Id = 999999,
                Name = "Updated Product",
                Description = "Updated description",
                Price = 25m,
                Stock = 75
            };

            var result = await productManager.EditProduct(model);

            Assert.False(result.Succeeded);
            Assert.Equal(ErrorEnum.NotFound, result.Error);

            productRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductExists_DeletesProduct()
        {
            var product = CreateProduct();

            productRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<ApplicationDbContext>(), product.Id))
                .ReturnsAsync(product);

            productRepositoryMock
                .Setup(x => x.DeleteAsync(It.IsAny<ApplicationDbContext>(), product))
                .ReturnsAsync(true);

            var result = await productManager.DeleteProduct(product.Id);

            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(product.Id, result.Data.Id);

            productRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<ApplicationDbContext>(), product), Times.Once);
        }

        [Fact]
        public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFound()
        {
            productRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<ApplicationDbContext>(), 999999))
                .ReturnsAsync((Product?)null);

            var result = await productManager.DeleteProduct(999999);

            Assert.False(result.Succeeded);
            Assert.Equal(ErrorEnum.NotFound, result.Error);

            productRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Never);
        }

        private static Product CreateProduct(int id = 100001, string name = "Product A", int stock = 100)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Description = "Test product",
                Price = 10m,
                Stock = stock,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}