
using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.AdminControllers.WishList
{
    /// <summary>
    /// Controller that exposes administrative endpoints for managing wishlists.
    /// </summary>
    /// <remarks>
    /// All endpoints in this controller require an authenticated user in the "Admin" role.
    /// This controller delegates work to an <see cref="IWishListService"/> and maps service results
    /// to <see cref="IActionResult"/> using <see cref="ActionResultStatus.MapResult"/>.
    /// </remarks>
    [Route("api/admin[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class WishListController : ControllerBase
    {
        /// <summary>
        /// Service used to perform wishlist operations.
        /// This dependency is provided via constructor injection.
        /// </summary>
        private readonly IWishListService _wishListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListController"/> class.
        /// </summary>
        /// <param name="wishListService">
        /// The wishlist service implementation used to handle wishlist operations.
        /// Must not be null; dependency injection should supply a valid instance.
        /// </param>
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }


        /// <summary>
        /// Deletes a wishlist identified by the provided wishlist identifier.
        /// </summary>
        /// <param name="wishListId">The identifier of the wishlist to delete.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the outcome of the operation.
        /// The result is produced by mapping the service <see cref="Result{Boolean}"/> to appropriate HTTP responses
        /// using <see cref="ActionResultStatus.MapResult"/>.
        /// </returns>
        /// <remarks>
        /// - Requires the caller to be an authenticated user in the "Admin" role.
        /// - On success, a mapped success response (typically 200/204 depending on mapping) is returned.
        /// - On failure, a mapped error response is returned with details in the response body.
        /// </remarks>
        [HttpDelete("DeleteWishList/{wishListId}")]
        public async Task<IActionResult> DeleteWishList(int wishListId)
        {
            var result = await _wishListService.DeleteWishList(wishListId);
            return ActionResultStatus.MapResult(result);
        }
    }
}
