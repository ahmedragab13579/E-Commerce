using Application.Services.InterFaces.Humans;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.AdminControllers.User
{
    [Route("api/admin[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _UserService;
        public UsersController(IUserService _UserService)
        {
            this._UserService = _UserService;
        }

        /// <summary>
        /// Fetches all Users.
        /// </summary>
        /// <returns>A list of all Users.</returns>
        /// <response code="200">Returns the list of Users.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _UserService.GetAll();

            return ActionResultStatus.MapResult(result);

        }
        [HttpGet("ActiveUsers/{min}")]
        /// <summary>
        /// Fetches all ActiveUsers.
        /// </summary>
        /// <returns>A list of all ActiveUsers.</returns>
        /// <response code="200">Returns the list of ActiveUsers.</response>
        public async Task<IActionResult> GetAllActiveUsers(int min)
        {
            var result = await _UserService.GetAllActiveUsers();

            return ActionResultStatus.MapResult(result);

        }
        /// <summary>
        /// Fetches a specific User by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the User.</param>
        /// <returns>The User details if found.</returns>
        /// <response code="200">Returns the requested User.</response>
        /// <response code="404">If the User with the specified ID is not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute]int id)
        {
            var result = await _UserService.GetUserWithOrders(id);
            return ActionResultStatus.MapResult(result);
        }
        /// <summary>
        /// Blocks an existing User.
        /// </summary>
        /// <param name="id">The ID of the User to Block.</param>
        /// <returns>No content if the Block is successful.</returns>
        /// <response code="204">If the User was Block successfully.</response>
        /// <response code="404">If the User to Block is not found.</response>

        [HttpPatch("Block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute] int id)
        {
            var result = await _UserService.BlockUser(id);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Actives an existing User.
        /// </summary>
        /// <param name="id">The ID of the User to Active.</param>
        /// <returns>No content if the Active is successful.</returns>
        /// <response code="204">If the User was Active successfully.</response>
        /// <response code="404">If the User to Active is not found.</response>

        [HttpPatch("Active/{id}")]
        public async Task<IActionResult> ActivateUser([FromRoute] int id)
        {
            var result = await _UserService.ActivateUser(id);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Fetches Most Active Users.
        /// </summary>
        /// <returns>A list of Most Active Users.</returns>
        /// <response code="200">Returns the list of Most Active Users.</response>

        [HttpGet("MostActiveUsers")]

        public async Task<IActionResult> GetMostActiveUsers([FromRoute] int count = 10)
        {
            var result = await _UserService.GetMustActiveUsers(count);
            return ActionResultStatus.MapResult(result);
        }









    }
}
