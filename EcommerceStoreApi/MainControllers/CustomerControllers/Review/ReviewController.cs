using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Review
{
    /// <summary>
    /// Controller that handles customer review-related operations.
    /// Exposes endpoints for adding, updating, deleting and retrieving reviews for customers.
    /// All endpoints require the "Customer" authorization policy.
    /// </summary>
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    public class ReviewController : ControllerBase
    {
        /// <summary>
        /// Service used to perform review operations.
        /// </summary>
        private readonly IReviewService _reviewService;

        /// <summary>
        /// Creates a new instance of <see cref="ReviewController"/>.
        /// </summary>
        /// <param name="reviewService">The review service to handle business logic for reviews.</param>
        public ReviewController(IReviewService _reviewService)
        {
            this._reviewService = _reviewService;
        }
            

        /// <summary>
        /// Adds a new review.
        /// </summary>
        /// <param name="addReviewDto">
        /// The review to add. This should contain the review summary, rating (1-5), the reviewer id and the product id.
        /// The DTO's validation attributes (e.g. <see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>, <see cref="System.ComponentModel.DataAnnotations.RangeAttribute"/>) are applied.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the add operation.
        /// The response will be mapped from the service <c>Result&lt;bool&gt;</c> via <see cref="ActionResultStatus.MapResult"/>.
        /// </returns>
        [HttpPost()]
        public async Task<IActionResult> Add([FromBody] Application.Dtos.Review.AddReviewDto addReviewDto)
        {
            var result = await _reviewService.Add(addReviewDto);
            return ActionResultStatus.MapResult(result);
        }


        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="id">The id of the review to update.</param>
        /// <param name="updateReviewDto">The updated review data (summary and rating).</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the update operation.
        /// The response will be mapped from the service <c>Result&lt;bool&gt;</c> via <see cref="ActionResultStatus.MapResult"/>.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Application.Dtos.Review.UpdateReviewDto updateReviewDto)
        {
            var result = await _reviewService.Update(id, updateReviewDto);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Deletes an existing review.
        /// </summary>
        /// <param name="id">The id of the review to delete.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> representing the result of the delete operation.
        /// The response will be mapped from the service <c>Result&lt;bool&gt;</c> via <see cref="ActionResultStatus.MapResult"/>.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _reviewService.Delete(id);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Retrieves all reviews submitted by a specific user.
        /// </summary>
        /// <param name="userId">The id of the user whose reviews should be retrieved.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of reviews for the specified user.
        /// The response will be mapped from the service <c>Result&lt;List&lt;ReviewDto&gt;&gt;</c> via <see cref="ActionResultStatus.MapResult"/>.
        /// </returns>
        [HttpGet("GetReviewsByUserId/{UserId}")]
        public async Task<IActionResult> GetReviewsByUserId([FromRoute] int userId)
        {
            var result = await _reviewService.GetReviewsByUserId(userId);
            return ActionResultStatus.MapResult(result);

        }
    }
}