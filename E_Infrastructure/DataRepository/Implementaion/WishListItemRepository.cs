using Application.DataReposatory.Interfaces;
using E_Domain.Models;
using E_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.DataRepository.Implementaion
{
    internal class WishListItemRepository : IWishListItemRepository
    {
        private readonly E_ApplicationDbContext _context;
        public WishListItemRepository(E_ApplicationDbContext context)
        {
            _context = context;
        }
        public  void RemoveRange(ICollection<WishlistItem> item)
        {
          _context.WishlistItems.RemoveRange(item);
        }

   


    }
}
