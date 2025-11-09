using Application.Dtos.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration; 
using System.Text.Json;

namespace E_Infrastructure.MiddleWare.Order_Amount_Service
{
    public class OrderQuantityValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly int _maxOrderQuantity;

        public OrderQuantityValidationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;

            _maxOrderQuantity = config.GetValue<int>("ValidationSettings:MaxOrderQuantity", 10); 
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/api/order", StringComparison.OrdinalIgnoreCase) 
                && context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.EnableBuffering();

                using (var memoryStream = new MemoryStream())
                {
                    await context.Request.Body.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;

                    var requestModel = await JsonSerializer.DeserializeAsync<OrderDto>(memoryStream,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (requestModel != null && requestModel.OrderItems.Any(n => n.Quantity > _maxOrderQuantity))
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await context.Response.WriteAsJsonAsync(new { message = $"Cannot order more than {_maxOrderQuantity} units of a single item." });
                        return;
                    }

                    memoryStream.Position = 0;

                    context.Request.Body = memoryStream;
                }
            }

            await _next(context);
        }
    }
}