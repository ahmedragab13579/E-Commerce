

using Application.Services.InterFaces;
using E_Infrastructure.ActionResultDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_Store_Api.MainControllers.AdminControllers.WishList
{
    /// <summary>
    /// Controller for managing wish lists in the admin area.
    /// </summary>
    /// <remarks>
    /// All endpoints require authentication and the "Admin" role.
    /// This controller delegates business logic to an <see cref="IWishListService"/>.
    /// </remarks>
    [Route("api/admin[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WishListController"/> class.
        /// </summary>
        /// <param name="wishListService">The service used to perform wish list operations.</param>
        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        /// <summary>
        /// Deletes a wish list by its identifier.
        /// </summary>
        /// <param name="wishListId">The identifier of the wish list to delete.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the outcome.
        /// The response is produced by <see cref="ActionResultStatus.MapResult{TResult}(Result{TResult})"/>
        /// which maps the service <see cref="Result{T}"/> to an appropriate HTTP response.
        /// </returns>
        /// <remarks>
        /// Calls <see cref="IWishListService.DeleteWishList(int)"/> internally.
        /// Possible outcomes:
        /// - Success: service returns Result.Ok(true) and the response will indicate success.
        /// - Failure: service returns Result.Fail(...) and the response will indicate the failure status and message.
        /// </remarks>
        [HttpDelete("DeleteWishList/{wishListId}")]
        public async Task<IActionResult> DeleteWishList(int wishListId)
        {
            var result = await _wishListService.DeleteWishList(wishListId);
            return ActionResultStatus.MapResult(result);
        }
    }
}
