using ProductManagement.Data.Models;

namespace ProductManagement.Business.Models
{
    public class ProductBusinessModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public static ProductBusinessModel FromDataModel(Product product)
        {
            return new ProductBusinessModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt
            };
        }

        public static Product ToDataModel(ProductBusinessModel productBusinessModel)
        {
            return new Product
            {
                Name = productBusinessModel.Name,
                Description = productBusinessModel.Description,
                Price = productBusinessModel.Price,
                Stock = productBusinessModel.Stock,
                CreatedAt = DateTime.UtcNow,
            };
        }
    }
}
