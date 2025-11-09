using Application.Dtos.CartItem;
using Application.Dtos.Review;
using Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.InterFaces
{
    public interface IReviewService
    {
        Task<Result<bool>> Add(AddReviewDto addReviewDto);
        Task<Result<bool>> Update(int id,UpdateReviewDto updateReviewDto);
        Task<Result<bool>> Delete(int id);
        Task<Result<List<ReviewDto>>> GetReviewsByProductId(int productId);

        Task<Result<List<ReviewDto>>> GetReviewsByUserId(int userId);
    }
}
