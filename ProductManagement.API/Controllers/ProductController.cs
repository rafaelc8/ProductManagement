using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Business.Contracts;
using ProductManagement.Business.Interface;
using ProductManagement.Business.Models;
using ProductManagement.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProductManagement.API.Controllers
{
    [Route("api/products")]
    [AllowAnonymous]
    [ApiController]
    public class ProductController : BaseController
    {
        public ProductController(
            IProductManager productManager)
            : base(productManager)
        {
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await productManager.GetAll();
            return SuccessResult(result.Data.Select(ProductDTO.FromBusinessModel).ToList());
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] ProductRequestDTO model)
        {
            var result = await productManager.CreateProduct(ProductRequestDTO.ToBusinessModel(model));

            if (!result.Succeeded)
                return GetErrorResult(result.Error);

            return SuccessResult(ProductDTO.FromBusinessModel(result.Data));
        }

        /// <summary>
        /// Gets a product by identifier.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await productManager.GetById(id);

            if (!result.Succeeded)
                return GetErrorResult(result.Error);

            return SuccessResult(ProductDTO.FromBusinessModel(result.Data));
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ProductRequestDTO model)
        {
            var product = ProductRequestDTO.ToBusinessModel(model);
            product.Id = id;

            var result = await productManager.EditProduct(product);

            if (!result.Succeeded)
                return GetErrorResult(result.Error);

            return SuccessResult(ProductDTO.FromBusinessModel(result.Data));
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await productManager.DeleteProduct(id);

            if (!result.Succeeded)
                return GetErrorResult(result.Error);

            return SuccessResult(new { message = "Product removed successfully." });
        }

        /// <summary>
        /// Decrements the available stock of a product.
        /// </summary>
        [HttpPost("{id:int}/decrement-stock/{quantity:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DecrementStock([FromRoute] int id, [FromRoute, Range(1, int.MaxValue)] int quantity)
        {
            var product = new ProductBusinessModel { Id = id, Stock = quantity };
            var result = await productManager.UpdateStockAsync(product, StockOperationEnum.Decrement);

            if (!result.Succeeded)
                return GetErrorResult(result.Error);

            return SuccessResult(new { message = "Stock decremented successfully." });
        }

        /// <summary>
        /// Adds stock to an existing product.
        /// </summary>
        [HttpPost("{id:int}/add-to-stock/{quantity:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddToStock([FromRoute] int id, [FromRoute, Range(1, int.MaxValue)] int quantity)
        {
            var product = new ProductBusinessModel { Id = id, Stock = quantity };
            var result = await productManager.UpdateStockAsync(product, StockOperationEnum.Add);

            if (!result.Succeeded)
                return GetErrorResult(result.Error);

            return SuccessResult(new { message = "Stock added successfully." });
        }

        /// <summary>
        /// Searches products by partial name.
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search([FromQuery, Required] string name)
        {
            var result = await productManager.GetByNameAsync(name);
            return SuccessResult(result.Data.Select(ProductDTO.FromBusinessModel).ToList());
        }

        /// <summary>
        /// Gets products whose stock is within the specified range.
        /// </summary>
        [HttpGet("stock-level")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByStockLevel([FromQuery, Range(0, int.MaxValue)] int min, [FromQuery, Range(0, int.MaxValue)] int max)
        {
            if (max < min)
                return BadRequest(new { message = "Max must be greater than or equal to min." });

            var result = await productManager.GetByStockLevelAsync(min, max);
            return SuccessResult(result.Data.Select(ProductDTO.FromBusinessModel).ToList());
        }
    }
}