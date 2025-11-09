using Application.DataReposatory.Interfaces.Orders;
using Application.Dtos.Order;
using Application.Results;
using E_Domain.Enums;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces.Ordres
{
    public interface IOrderService
    {
        Task<Result<int>> CheckOut( );
        Task<Result<ICollection<OrderDto>>> GetAll();
        Task<Result<ICollection<OrderDto>>> GetOrdersByUserId();
        Task<Result<ICollection<OrderDto>>> GetFilteredOrders(OrderStatus status);
        Task<Result<bool>> Update(int id,UpdateOrderStatusDto Order);
        Task<Result<OrderDto>> GetOrderById(int Orderid);
    }
}
