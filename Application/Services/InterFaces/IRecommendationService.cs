using Application.Dtos.ProductsRecommendations;
using Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces
{
    public interface IRecommendationService
    {
        Task<Result<List<RecommendationItemDto>?>> GetRecommendationsAsync(int productId);
    }
}
