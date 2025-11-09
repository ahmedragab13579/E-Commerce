using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces
{
    public interface IWishListRepository:IGenericRepository<E_Domain.Models.Wishlist>
    {

        Task<Wishlist> GetOrCreateWishlistAsync(int userId);

    }
}
