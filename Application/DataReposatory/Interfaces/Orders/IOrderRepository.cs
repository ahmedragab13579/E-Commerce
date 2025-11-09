using Application.Results;
using Application.Services.InterFaces.Ordres;
using E_Domain.Enums;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces.Orders
{
    public interface IOrderRepository : IGenericRepository<Order>
    {

        Task<Order> GetOrderIdIncludeOrderItemsAsync(int OrderId);
     
        Task<IEnumerable<Order>> GetFilteredOrdersAsync(OrderStatus  status);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int UserId);


    }
}
