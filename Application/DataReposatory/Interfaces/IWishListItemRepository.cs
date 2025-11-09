using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces
{
    public interface IWishListItemRepository
    {
        void  RemoveRange(ICollection<WishlistItem> item);
    }
}
