using ProductManagement.Business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Business.Contracts
{
    public class ProductRequestDTO
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(typeof(decimal), "0.01", "999999999", ParseLimitsInInvariantCulture = true)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public static ProductBusinessModel ToBusinessModel(ProductRequestDTO product)
        {
            return new ProductBusinessModel
            {
                
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }
    }
}
