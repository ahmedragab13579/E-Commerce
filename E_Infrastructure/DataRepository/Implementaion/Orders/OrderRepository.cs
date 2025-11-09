using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.Orders;
using Application.Services.InterFaces.Ordres;
using E_Domain.Enums;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion.Orders
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbSet<Order> _orders;
        public OrderRepository(E_ApplicationDbContext _context):base(_context)
        {
            _orders = this._context.Orders;
            
        }
   
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int UserId)
        {
            return await _orders.Where(n => n.UserId == UserId).ToListAsync() ;  
        }
        public async Task <Order> GetOrderIdIncludeOrderItemsAsync(int OrderId)
        {
            return await _orders.Include(n=>n.OrderItems).FirstOrDefaultAsync(n => n.Id == OrderId) ;  
        }
    
        public async Task<IEnumerable<Order>> GetFilteredOrdersAsync(OrderStatus status)
        {
            return await _orders.AsNoTracking().Where(n => n.Status == status).ToListAsync();
        }
    }
}
