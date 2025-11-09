using Application.DataReposatory.Interfaces;
using Application.DataReposatory.Interfaces.Carts;
using Application.Dtos.CartItem;
using Application.Results;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion.Carts
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
     
        private readonly DbSet<Cart> _carts;
        public CartRepository(E_ApplicationDbContext _context):base(_context)
        {
            _carts = _context.Set<Cart>();
        }


    
        public async Task<E_Domain.Models.Cart> GetCartByUserIdAsync(int UserId)
        {
            return await _carts.AsNoTracking().Include(n=>n.CartItems).FirstOrDefaultAsync(n => n.UserId==UserId);
        }



   
    }
}
