using Application.Dtos.CartItem;
using Application.Results;
using Application.Services.InterFaces.CartSession;
using E_Domain.Enums;
using E_Domain.Models;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Carts
{
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy ="Customer")]
    public class CartController : ControllerBase
    {
        private readonly ICartSessionService _CartSessionService;
        public CartController(ICartSessionService _CartSessionService)
        {
            this._CartSessionService = _CartSessionService;
        }

        /// <summary>
        /// Deletes a CartItem by its ProductID.
        /// </summary>
        /// <param name="Productid">The ID of the CartItem to delete.</param>
        /// <returns>No content if the deletion is successful.</returns>
        /// <response code="204">If the CartItem was deleted successfully.</response>
        /// <response code="404">If the CartItem to delete is not found.</response>
        /// 

        [HttpDelete("{Productid}")]

       public async Task<IActionResult> DeleteItem([FromRoute]int Productid)
        {
            Result<bool> result = await _CartSessionService.RemoveItemAsync( Productid);
            return ActionResultStatus.MapResult(result);
        }
        /// <summary>
        /// Fetches User Cart .
        /// </summary>
        /// <returns>A User Cart If Exist.</returns>
        /// <response code="200">Returns the User Cart. </response>
        [HttpGet]

        public  async Task<IActionResult> GetUserCart()
        {
            return Ok( await  _CartSessionService.GetCartAsync());
        }
        /// <summary>
        /// Adds a new CartItem.
        /// </summary>
        /// <param name="item">The CartItem data to add.</param>
        /// <returns>The newly created CartItem.</returns>
        /// <response code="201">Returns the newly created CartItem.</response>
        /// <response code="400">If the CartItem data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> AddItem([FromBody]AddCartItemDto item)
        {
            var result = await _CartSessionService.AddItemAsync(item);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Updates an existing CartItem.
        /// </summary>
        /// <param name="item">The updated CartItem data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the CartItem was updated successfully.</response>
        /// <response code="404">If the CartItem to update is not found.</response>
        [HttpPut]

        public async Task<IActionResult> UpdateItem([FromBody] UpdateCartItemDto item)
        {
            var result=await _CartSessionService.UpdateItemAsync(item);
            return ActionResultStatus.MapResult(result);

        }




    }

}
