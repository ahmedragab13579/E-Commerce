using Application.Dtos.CartItem;
using Application.Results;
using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces.Carts
{
    public interface ICartRepository : IGenericRepository<E_Domain.Models.Cart>
    {
        Task<E_Domain.Models.Cart> GetCartByUserIdAsync(int UserId);
    
    }
}
