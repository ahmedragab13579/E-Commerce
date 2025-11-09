using Application.Dtos.Order;
using Application.Results;
using Application.Services.InterFaces.Ordres;
using E_Domain.Enums;
using E_Infrastructure.ActionReslutDict;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EcommerceStoreApi.MainControllers.AdminControllers.Orders
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService _orderService)
        {
            this._orderService = _orderService;
        }


        /// <summary>
        /// Updates an existing Order.
        /// </summary>
        /// <param name="OrderId">The ID of the Order to update.</param>
        /// <param name="Order">The updated Order data.</param>
        /// <returns>No content if the update is successful.</returns>
        /// <response code="204">If the Order was updated successfully.</response>
        /// <response code="404">If the Order to update is not found.</response>
        [HttpPatch("{OrderId}")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int OrderId,[FromBody]UpdateOrderStatusDto Order)
        {

            Result<bool> result = await _orderService.Update(OrderId,Order);
            return ActionResultStatus.MapResult(result);
        }
        /// <summary>
        /// Fetches all Orders.
        /// </summary>
        /// <returns>A list of Spacific  Orders With Status.</returns>
        /// <response code="200">Returns the list of Spacific  Orders.</response>


        [HttpGet("Filterd/{status}")]
        public async Task<IActionResult> FilteredOrders([FromRoute] OrderStatus status)
        {
            Result<ICollection<OrderDto>> result = await _orderService.GetFilteredOrders(status);
            return ActionResultStatus.MapResult(result);
        }

        /// <summary>
        /// Fetches all Orders.
        /// </summary>
        /// <returns>A list of all Orders.</returns>
        /// <response code="200">Returns the list of Orders.</response>
        [HttpGet]
        public async Task<IActionResult> GetAll(  )
        {
            Result<ICollection<OrderDto>> result = await _orderService.GetAll();
            return ActionResultStatus.MapResult(result);
        }



        /// <summary>
        /// Fetches a specific Order by its ID.
        /// </summary>
        /// <param name="OrderId">The unique identifier of the Order.</param>
        /// <returns>The Order details if found.</returns>
        /// <response code="200">Returns the requested Order.</response>
        /// <response code="404">If the Order with the specified ID is not found.</response>

        [HttpGet("{OrderId}")]

        public async Task<IActionResult> GetOrderById([FromRoute] int OrderId)
        {
            Result<OrderDto> result = await _orderService.GetOrderById(OrderId);
            return ActionResultStatus.MapResult(result);
        }







    }
}
