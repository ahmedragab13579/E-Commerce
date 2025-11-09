using Application.Dtos.User;
using Application.Services.InterFaces.Humans;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Customer
{
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly IUserService userService;
        public CustomersController(IUserService userService)
        {
            this.userService = userService;
        }
        /// <summary>
        /// Edit Profile Of a Customer .
        /// </summary>
        /// <param name="updateUserDto">The updated Customer data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the Customer was updated successfully.</response>
        /// <response code="404">If the Customer to update is not found.</response>

        [HttpPatch("EditProfile")]

        public async Task<IActionResult> EditProfile([FromForm] UpdateUserDto updateUserDto)
        {
            var result = await userService.Update(updateUserDto);
            return ActionResultStatus.MapResult(result);

        }

        /// <summary>
        /// ChangePassword Of a Customer .
        /// </summary>
        /// <param name="OldPassword">The Old  Password Customer data.</param>
        /// <param name="NewPassword">The New Password Customer data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the Customer was updated successfully.</response>
        /// <response code="404">If the Customer to update is not found.</response>

        [HttpPatch("ChangePassword/NewPassword/{NewPassword}/OldPassword/{OldPassword}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string OldPassword, [FromRoute] string NewPassword)
        {
            var result = await userService.ChangePassword( OldPassword, NewPassword);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// ChangePassword Of a Customer .
        /// </summary>
        /// <param name="Email">The Old  Email Customer data.</param>
        /// <param name="NewPassword">The New Password Customer data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the Customer was updated successfully.</response>
        /// <response code="404">If the Customer to update is not found.</response>
        [HttpPatch("ForgotPassword/Email/{Email}/NewPassword/{NewPassword}")]

        public async Task<IActionResult> ForgotPassword([FromRoute] string Email, [FromRoute] string NewPassword)
        {
            var result = await userService.ForgotPassword(Email, NewPassword);
            return ActionResultStatus.MapResult(result);
        }
    }
}
