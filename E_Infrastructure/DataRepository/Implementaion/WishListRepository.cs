using Application.DataReposatory.Interfaces; 
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion
{
    public class WishListRepository : GenericRepository<Wishlist>, IWishListRepository
    {
        private readonly E_ApplicationDbContext _context; 

        public WishListRepository(E_ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    
        public async Task<Wishlist> GetOrCreateWishlistAsync(int userId)
        {
            var wishlist = await _context.Wishlists
                                         .Include(w => w.WishlistItems)
                                         .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist(userId);
                await _context.Wishlists.AddAsync(wishlist);
                await _context.SaveChangesAsync();
            }
            return wishlist;
        }



    }
}