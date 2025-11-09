using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Products
{
    /// <summary>
    /// API controller that exposes product-related endpoints for authenticated customers.
    /// Routes are prefixed with "api/shop/[controller]". All endpoints require the "Customer" policy.
    /// Responsible for retrieving product listings, filtered product queries, and product recommendations.
    /// </summary>
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly IRecommendationService _recommendationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="ProductService">Service used to query and retrieve product data.</param>
        /// <param name="recommendationService">Service used to fetch product recommendations.</param>
        public ProductsController(IProductService _ProductService, IRecommendationService _recommendationService)
        {
            this._ProductService = _ProductService;
            this._recommendationService = _recommendationService;
        }

        /// <summary>
        /// Fetches all products available in the store.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a collection of products. Results are mapped through the action result helper.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the list of products on success (HTTP 200).
        /// The concrete result type and status are determined by the underlying <see cref="IProductService"/>.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _ProductService.GetAll();
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Fetches products that match the specified name filter.
        /// </summary>
        /// <param name="Name">The unique or partial product name to filter results by.</param>
        /// <remarks>
        /// If no products match the provided name, an appropriate not-found or empty result will be returned
        /// according to the behavior of <see cref="IProductService.GetAllWithFilter(string)"/>.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the filtered product(s) (HTTP 200) or a not-found response (HTTP 404)
        /// if the service indicates no match.
        /// </returns>
        [HttpGet("{Name}")]
        public async Task<IActionResult> GetProducts([FromRoute] string Name)
        {
            var result = await _ProductService.GetAllWithFilter(Name);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Retrieves product recommendations for the specified product identifier.
        /// </summary>
        /// <param name="id">The identifier of the product to generate recommendations for.</param>
        /// <remarks>
        /// Recommendations are produced by the injected <see cref="IRecommendationService"/> and returned
        /// through the standard action result mapping helper.
        /// </remarks>
        /// <returns>
        /// An <see cref="IActionResult"/> containing recommended products (HTTP 200) or an appropriate error status
        /// if the recommendation service fails or the product is not found.
        /// </returns>
        [HttpGet("{id}/recommendations")]
        public async Task<IActionResult> GetProductRecommendations(int id)
        {
            var recommendations = await _recommendationService.GetRecommendationsAsync(id);
            return ActionResultStatus.MapResult(recommendations);
        }
    }
}
