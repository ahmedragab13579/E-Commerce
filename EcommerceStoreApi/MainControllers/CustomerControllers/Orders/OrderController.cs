using Application.Results;
using Application.Services.InterFaces.Ordres;
using E_Infrastructure.ActionReslutDict;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Orders
{
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize]
    [Authorize(Policy = "Customer")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        public OrdersController(IOrderService _OrderService)
        {
            this._OrderService = _OrderService;
        }
        /// <summary>
        ///Check Out The Order
        /// </summary>
        /// <returns>No content if the Check Out is successful.</returns>
        /// <response code="204">If the Order was Check Out successfully.</response>
        /// <response code="404">If the Order to  Check Out is not found.</response>
        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOUt()
        {
            Result<int> result = await _OrderService.CheckOut();
            return ActionResultStatus.MapResult(result);
        }

    }
}
