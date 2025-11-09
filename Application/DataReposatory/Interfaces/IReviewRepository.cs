using E_Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataReposatory.Interfaces
{
    public interface IReviewRepository:IGenericRepository<E_Domain.Models.Review>
    {
        Task<bool> UserHasReviewedProductAsync(int userId, int productId);
        Task<List<Review>> GetReviewsByUserIdAsync(int userId);
        Task<List<Review>> GetReviewsByProductIdAsync(int productId);

    }
}
