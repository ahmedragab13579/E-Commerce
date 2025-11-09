using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.Orders;
using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.CartItem;
using Application.Dtos.Carts;
using Application.Dtos.Order;
using Application.Dtos.OrderItem;
using Application.Dtos.Product;
using Application.Results;
using Application.Services.InterFaces.CartSession;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Mapping;
using Application.Services.InterFaces.Ordres;
using E_Domain.Enums;
using E_Domain.Models;
using E_Infrastructure.Services.Implementaions.Mapping;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedMessages;
using System.Diagnostics.Eventing.Reader;
using System.Transactions;


namespace E_Infrastructure.Services.Implementaions.Orders
{
    public class OrderService : IOrderService
    {

        private readonly IMappingServices _mappingServices;
        private readonly ICartSessionService _cartSessionService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBus _bus;
        public OrderService(IBus _bus,IUnitOfWork unitOfWork,ICurrentUserService _currentUserService,ICartSessionService _cartSessionService, IMappingServices _mappingServices)
        {
            this._mappingServices = _mappingServices;
            this._cartSessionService = _cartSessionService;
            this._currentUserService = _currentUserService;
            this.unitOfWork = unitOfWork;
            this._bus = _bus;
        }

        public async Task<Result<int>> CheckOut()
        {
            try
            {
                var cartResult = await _cartSessionService.GetCartAsync();
                if (!cartResult.Success || !cartResult.Data.CartItems.Any())
                {
                    return Result<int>.Fail("EMPTY_CART", "Your cart is empty.");
                }

                var orderItemsResult = await CreateOrderItemsAndValidateStockAsync(cartResult.Data);
                if (!orderItemsResult.Success)
                {
                    return Result<int>.Fail(orderItemsResult.Code, orderItemsResult.Message);
                }
                var orderItems = orderItemsResult.Data;
                List<CartItem> cartItems = (orderItems).ToList();
                var newOrder = CreateOrderEntityAsync(cartItems);

                await unitOfWork.Orders.AddAsync(newOrder);
                var affectedRows = await unitOfWork.CompleteTask();

                if (affectedRows > 0)
                {
                    await _cartSessionService.ClearSessionCartAsync();
                    string? email = _currentUserService.GetUserEmail();
                    string? userName = _currentUserService.GetUserName();
                    OrderPlacedEvent Messsage = new OrderPlacedEvent(newOrder.Id,userName, newOrder.TotalAmount, DateTime.UtcNow, email);
                   await _bus.Publish(Messsage);

                    return Result<int>.Ok(newOrder.Id, "Order placed successfully.");
                    
                }

                return Result<int>.Fail("SAVE_FAILED", "An error occurred while saving the order.");
            }
            catch(ArgumentException argEx)
            {
                return Result<int>.Fail("INVALID_DATA", argEx.Message);
            }
            catch (Exception ex)
            {
                return Result<int>.Fail("CHECKOUT_FAILED", ex.Message);
            }
           
        }


        private async Task<Result<List<CartItem>>> CreateOrderItemsAndValidateStockAsync(CartDto cart)
        {
            var productIds = cart.CartItems.Select(ci => ci.ProductId).ToList();
            var productsFromDb = await unitOfWork.Products.GetProductsByIdsAsync(productIds);
            var orderItems = new List<CartItem>();
            try
            {
                foreach (var cartItem in cart.CartItems)
                {
                    var product = productsFromDb.FirstOrDefault(p => p.Id == cartItem.ProductId);
                    if (product == null)
                        return Result<List<CartItem>>.Fail("PRODUCT_NOT_FOUND", $"Product with ID {cartItem.ProductId} not found.");
                    orderItems.Add(new CartItem(cart.Id,product, cartItem.Quantity));
                }

            }
            catch (Exception ex)
            {
                return Result<List<CartItem>>.Fail("STOCK_UPDATE_FAILED", ex.Message);
            }

            return Result<List<CartItem>>.Ok(orderItems);
        }

        private  Order CreateOrderEntityAsync(List<CartItem> CartItems)
        {
            int? userId = _currentUserService.GetCurrentUserId();
            if (userId == null)
            {
                throw new ArgumentException("User not found.");
            }
            return new Order(userId.Value, CartItems);
        }

        public async Task<Result<ICollection<OrderDto>>> GetFilteredOrdersAsync(OrderStatus status)
        {
            var orders = await unitOfWork.Orders.GetFilteredOrdersAsync(status);
            if (orders!=null && orders.Any())
            {
                return Result<ICollection<OrderDto>>.Ok(_mappingServices.MapList<Order, OrderDto>(orders).ToList(), "Orders Were Found");
            }
            return Result<ICollection<OrderDto>>.Fail("NOT_FOUND", "No Orders Found");
        }

      

        public async Task<Result<ICollection<OrderDto>>> GetOrdersByUserId()
        {
            int? UserId =  _currentUserService.GetCurrentUserId();
            if (UserId == null)
            {
                return Result<ICollection<OrderDto>>.Fail("USER_NOT_FOUND", "User Not Found");
            }
            var orders = await unitOfWork.Orders.GetOrdersByUserIdAsync(UserId.Value);
            if (orders!=null && orders.Any())
            {
                return Result<ICollection<OrderDto>>.Ok(_mappingServices.MapList<Order, OrderDto>(orders).ToList(), "Orders Were Found");
            }
            return Result<ICollection<OrderDto>>.Fail("NOT_FOUND", "No Orders Found");
        }

        public async Task<Result<bool>> Update(int id,UpdateOrderStatusDto orderDto)
        {
            var order = await unitOfWork.Orders.GetOrderIdIncludeOrderItemsAsync(id);
            if (order == null)
            {
                return Result<bool>.Fail("NOT_FOUND", "Order Not Found");
            }
            try
            {
                if(orderDto.NewStatus == OrderStatus.Cancelled.ToString() && order.Status!= OrderStatus.Cancelled)
                {
                    List<int> productIds = order.OrderItems.Select(oi => oi.ProductId).ToList();
                    var products = await unitOfWork.Products.GetProductsByIdsAsync(productIds);
                    foreach (var item in order.OrderItems)
                    {
                        var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                        if (product != null)
                        {
                            product.IncreaseStock(item.Quantity);
                        }
                    }
                    unitOfWork.Products.UpdateRange(products);
                }
                Enum.TryParse<OrderStatus>(orderDto.NewStatus, out var newStatus);
                order.UpdateStatus(newStatus);
                unitOfWork.Orders.Update(order);

                var affectedRows = await unitOfWork.CompleteTask();

                return Result<bool>.Ok(affectedRows > 0, "Order status updated successfully.");

            }
            catch (InvalidOperationException ex)
            {
                return Result<bool>.Fail("UPDATE_FAILED", ex.Message);
            }

        }
        public async Task<Result<OrderDto>> GetOrderById(int OrderId)
        {
            Order order = await unitOfWork.Orders.GetByIdAsync(OrderId);
            if (order!=null)
            {
                return Result<OrderDto>.Ok(_mappingServices.Map<Order, OrderDto>(order), "User Was Found");
            }
            return Result<OrderDto>.Fail("NOT_FOUND", "Order Not Found");
        }

        public async Task<Result<ICollection<OrderDto>>> GetFilteredOrders(OrderStatus status)
        {
            var orders = await unitOfWork.Orders.GetFilteredOrdersAsync(status);
            if (orders != null && orders.Any())
            {
                return (Result<ICollection<OrderDto>>.Ok(_mappingServices.MapList<Order, OrderDto>(orders).ToList(), "Orders Were Found"));
            }
            return (Result<ICollection<OrderDto>>.Fail("NOT_FOUND", "No Orders Found") );

        }
        public async Task<Result<ICollection<OrderDto>>> GetAll( )
        {
            var orders = await unitOfWork.Orders.GetAllAsync();
            if (orders != null && orders.Any())
            {
                return (Result<ICollection<OrderDto>>.Ok(_mappingServices.MapList<Order, OrderDto>(orders).ToList(), "Orders Were Found"));
            }
            return (Result<ICollection<OrderDto>>.Fail("NOT_FOUND", "No Orders Found") );

        }
    }
}
