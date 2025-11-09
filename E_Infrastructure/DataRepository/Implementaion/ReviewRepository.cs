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
    public class ReviewRepository : GenericRepository<Review>,IReviewRepository
    {
        private readonly DbSet<Review> _reviewSet;
        public ReviewRepository(E_ApplicationDbContext _context) : base(_context)
        {
            _reviewSet = this._context.Reviews;

        }

        public async Task<bool> UserHasReviewedProductAsync(int userId, int productId)
        {
            return await _context.Reviews
                                 .AnyAsync(r => r.ReviewerId == userId && r.ProductId == productId);
        }



        public async Task<List<Review>> GetReviewsByUserIdAsync(int userId)
        {
            return await _reviewSet
                         .Where(r => r.ReviewerId == userId)
                         .ToListAsync();
        }

        public async Task<List<Review>> GetReviewsByProductIdAsync(int productId)
        {
            return await _reviewSet
                         .Where(r => r.ProductId == productId)
                         .ToListAsync();
        }
    }
}
