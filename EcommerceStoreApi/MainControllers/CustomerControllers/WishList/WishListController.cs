using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceStoreApi.MainControllers.CustomerControllers.WishList
{
    /// <summary>
    /// Controller that exposes wishlist operations for customers.
    /// </summary>
    /// <remarks>
    /// Routes in this controller are prefixed with "api/shop/wishlist". 
    /// Access to all endpoints requires the "Customer" authorization policy.
    /// Each action delegates business logic to an injected <see cref="IWishListService"/> and maps the returned <see cref="Result{T}"/>
    /// to an <see cref="IActionResult"/> using <see cref="ActionResultStatus.MapResult{T}(Result{T})"/>.
    /// </remarks>
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    public class WishListController : ControllerBase
    {
        /// <summary>
        /// Service used to perform wishlist operations (add/remove/clear/get).
        /// </summary>
        private readonly IWishListService _wishListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListController"/> class.
        /// </summary>
        /// <param name="wishListService">The wishlist service used to perform business operations.</param>
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        /// <summary>
        /// Adds the specified product to the current user's wishlist.
        /// </summary>
        /// <param name="productId">The identifier of the product to add to the wishlist. Must be a positive integer.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that represents the outcome of the add operation.
        /// The underlying service returns a <see cref="Result{Boolean}"/> which is mapped to an appropriate HTTP response
        /// by <see cref="ActionResultStatus.MapResult{T}(Result{T})"/>.
        /// </returns>
        /// <remarks>
        /// This endpoint expects that the caller is authenticated and authorized by the "Customer" policy.
        /// Typical responses:
        /// - 200 OK: operation succeeded (Result.Success == true).
        /// - 400/404/500: mapped from the service <see cref="Result{T}"/> failure information.
        /// </remarks>
        [HttpPost("AddToWishList/{productId}")]
        public async Task<IActionResult> AddToWishList([FromRoute] int productId)
        {
            var result = await _wishListService.AddToWishList(productId);
            return ActionResultStatus.MapResult(result);
        }


        /// <summary>
        /// Retrieves the wishlist for the current authenticated user.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the user's wishlist data when successful.
        /// The underlying service returns a <see cref="Result{WishListDto}"/> which is mapped to an appropriate HTTP response
        /// by <see cref="ActionResultStatus.MapResult{T}(Result{T})"/>.
        /// </returns>
        /// <remarks>
        /// The user identity is expected to be resolved by the application (no userId route param is required).
        /// </remarks>
        [HttpGet("GetWishListByUserId")]
        public async Task<IActionResult> GetWishListByUserId()
        {
            var result = await _wishListService.GetWishListByUserId();
            return ActionResultStatus.MapResult(result);
        }



        /// <summary>
        /// Removes the specified product from the current user's wishlist.
        /// </summary>
        /// <param name="productId">The identifier of the product to remove from the wishlist. Must be a positive integer.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that indicates whether the removal was successful.
        /// The underlying service returns a <see cref="Result{Boolean}"/> which is mapped to an appropriate HTTP response
        /// by <see cref="ActionResultStatus.MapResult{T}(Result{T})"/>.
        /// </returns>
        /// <remarks>
        /// Typical responses:
        /// - 200 OK: product removed.
        /// - 404 Not Found: product was not in the wishlist.
        /// </remarks>
        [HttpDelete("RemoveFromWishList/{productId}")]
        public async Task<IActionResult> RemoveFromWishList([FromRoute] int productId)
        {
            var result = await _wishListService.RemoveFromWishList(productId);
            return ActionResultStatus.MapResult(result);
        }



        /// <summary>
        /// Clears all items from the current user's wishlist.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that indicates whether the clear operation was successful.
        /// The underlying service returns a <see cref="Result{Boolean}"/> which is mapped to an appropriate HTTP response
        /// by <see cref="ActionResultStatus.MapResult{T}(Result{T})"/>.
        /// </returns>
        /// <remarks>
        /// This operation removes all wishlist items for the authenticated user.
        /// </remarks>
        [HttpDelete("ClearWishList")]
        public async Task<IActionResult> ClearWishList()
        {
            var result = await _wishListService.ClearWishList();
            return ActionResultStatus.MapResult(result);
        }
    }
}