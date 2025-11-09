using Application.Dtos.Payment;
using Application.Results;
using Application.Services.InterFaces;
using E_Infrastructure.ActionReslutDict;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceStoreApi.MainControllers.CustomerControllers.Payment
{
    [Route("api/shop/[controller]")]
    [ApiController]
    [Authorize(Policy = "Customer")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService _paymentService)
        {
            this._paymentService = _paymentService;
            
        }
        /// <summary>
        /// Adds a new Payment.
        /// </summary>
        /// <param name="Payment">The Payment data to add.</param>
        /// <returns>The newly created Payment.</returns>
        /// <response code="201">Returns the newly created Payment.</response>
        /// <response code="400">If the Payment data is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]AddPaymentDto Payment)
        {
            Result<int> result = await _paymentService.Add(Payment);
            return ActionResultStatus.MapResult(result);
        }
    }
}
