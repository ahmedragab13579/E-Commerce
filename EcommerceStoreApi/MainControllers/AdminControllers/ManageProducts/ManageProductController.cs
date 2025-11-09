using Application.Dtos.Product;
using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.AdminControllers.ManageProducts
{
    [Route("api/admin/products")] 
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class ProductsController : ControllerBase 
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Fetches all Products.
        /// </summary>
        /// <returns>A list of all Products.</returns>
        /// <response code="200">Returns the list of Products.</response>
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromRoute] string? name, [FromRoute] int? categoryId)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                var filteredResult = await _productService.GetAllWithFilter(name);
                return ActionResultStatus.MapResult(filteredResult);
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                var categoryResult = await _productService.GetProductsByCategoryId(categoryId.Value);
                return ActionResultStatus.MapResult(categoryResult);
            }

            var allResult = await _productService.GetAll();
            return ActionResultStatus.MapResult(allResult);
        }

        /// <summary>
        /// Get The Most Selling Of  Products.
        /// </summary>
        /// <returns>A list of Most Selling Products.</returns>
        /// <response code="200">Returns the list of Most Selling Products.</response>
        [HttpGet("GetTheMostSelling")]


        public async Task<IActionResult> GetTheMostSellingProducts()
        {
            var result = await _productService.GetBestSellingProductsAsync();
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Fetches a specific Product by its ID.
        /// </summary>
        /// <param name="productId">The unique identifier of the Product.</param>
        /// <returns>The Product details if found.</returns>
        /// <response code="200">Returns the requested Product.</response>
        /// <response code="404">If the Product with the specified ID is not found.</response>
        [HttpGet("{productId}")] 
        public async Task<IActionResult> GetProductById(int productId)
        {
            var result = await _productService.GetById(productId);
            return ActionResultStatus.MapResult(result);
        }
        /// <summary>
        /// Adds a new Product.
        /// </summary>
        /// <param name="productDto">The Product data to add.</param>
        /// <returns>The newly created Product.</returns>
        /// <response code="201">Returns the newly created Product.</response>
        /// <response code="400">If the Product data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] AddProductDto productDto)
        {
            var result = await _productService.Add(productDto);
            return ActionResultStatus.MapResult(result);
        }
        /// <summary>
        /// Updates an existing Product.
        /// </summary>
        /// <param name="productId">The ID of the Product to update.</param>
        /// <param name="productDto">The updated Product data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the Product was updated successfully.</response>
        /// <response code="404">If the Product to update is not found.</response>
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] UpdateProductDto productDto)
        {
            if (productId < 1)
            {
                return BadRequest("ID Must be Positive number.");
            }

            var result = await _productService.Update(productId,productDto);
            return ActionResultStatus.MapResult(result);
        }
        /// <summary>
        /// Deletes a Product by its ID.
        /// </summary>
        /// <param name="productId">The ID of the Product to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the Product was deleted successfully.</response>
        /// <response code="404">If the Product to delete is not found.</response>
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var result = await _productService.DeleteAsync(productId);
            return ActionResultStatus.MapResult(result);
        }
    }

}

