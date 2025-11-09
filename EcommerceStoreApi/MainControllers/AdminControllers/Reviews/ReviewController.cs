using Application.Dtos.Review;
using Application.Results;
using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceStoreApi.MainControllers.AdminControllers.Reviews
{
    /// <summary>
    /// Manages product reviews. Requires Administrator access.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        /// <summary>
        /// Initializes a new instance of the ReviewController.
        /// </summary>
        /// <param name="reviewService">The review service.</param>
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        /// <summary>
        /// Deletes a specific review by its ID. (Admin Only)
        /// </summary>
        /// <param name="id">The ID of the review to delete.</param>
        /// <returns>A result indicating the success or failure of the deletion.</returns>
        /// <response code="200">The operation result (success or failure).</response>
        /// <response code="404">The review was not found.</response>
        /// <response code="401">Unauthorized (User not authenticated).</response>
        /// <response code="403">Forbidden (User is not an Admin).</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteReview([FromRoute] int id)
        {
            var result = await _reviewService.Delete(id);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Gets all reviews for a specific product. (Admin Only)
        /// </summary>
        /// <param name="productId">The ID of the product.</param>
        /// <returns>A list of reviews for the specified product.</returns>
        /// <response code="200">Returns the list of reviews (can be empty).</response>
        /// <response code="404">The product was not found.</response>
        /// <response code="401">Unauthorized (User not authenticated).</response>
        /// <response code="403">Forbidden (User is not an Admin).</response>
        [HttpGet("GetReviewsByProductId/{productId}")]
        [ProducesResponseType(typeof(Result<List<ReviewDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<ReviewDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReviewsByProductId([FromRoute] int productId)
        {
            var result = await _reviewService.GetReviewsByProductId(productId);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Gets all reviews written by a specific user. (Admin Only)
        /// </summary>
        /// <param name="userId">The ID of the user (reviewer).</param>
        /// <returns>A list of reviews written by the specified user.</returns>
        /// <response code="200">Returns the list of reviews (can be empty).</response>
        /// <response code="404">The user was not found.</response>
        /// <response code="401">Unauthorized (User not authenticated).</response>
        /// <response code="403">Forbidden (User is not an Admin).</response>
        [HttpGet("GetReviewsByUserId/{userId}")]
        [ProducesResponseType(typeof(Result<List<ReviewDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Result<List<ReviewDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetReviewsByUserId([FromRoute] int userId)
        {
            var result = await _reviewService.GetReviewsByUserId(userId);
            return ActionResultStatus.MapResult(result);
        }
    }
}