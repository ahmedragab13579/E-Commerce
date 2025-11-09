using Application.DataReposatory.Interfaces.UnitOfWork;
using Application.Dtos.Review;
using Application.Results;
using Application.Services.InterFaces;
using Application.Services.InterFaces.Humans;
using Application.Services.InterFaces.Mapping;
using E_Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMappingServices _mappingServices;
        private readonly ICurrentUserService _currentUserService;

        public ReviewService(IUnitOfWork unitOfWork, IMappingServices mappingServices, ICurrentUserService currentUserService )
        {
            _unitOfWork = unitOfWork;
            _mappingServices = mappingServices;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Add(AddReviewDto addReviewDto)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var user = await _unitOfWork.Users.GetByIdAsync(currentUserId.Value);
                if (user == null || user.IsDeleted || user.IsBlocked)
                {
                    return Result<bool>.Fail("INVALID_USER", "User account is invalid, blocked, or deleted.");
                }

                var product = await _unitOfWork.Products.GetByIdAsync(addReviewDto.ProductId);
                if (product == null || product.IsDeleted == true)
                {
                    return Result<bool>.Fail("PRODUCT_NOT_FOUND", "Product not found or has been deleted.");
                }

                bool alreadyReviewed = await _unitOfWork.reviewRepository.UserHasReviewedProductAsync(currentUserId.Value, addReviewDto.ProductId);
                if (alreadyReviewed)
                {
                    return Result<bool>.Fail("DUPLICATE_REVIEW", "You have already reviewed this product.");
                }

                var newReview = new Review(addReviewDto.SummaryReview, addReviewDto.Rating, user, product);

                await _unitOfWork.reviewRepository.AddAsync(newReview);
                int success = await _unitOfWork.CompleteTask();

                if (success > 0)
                {
                    return Result<bool>.Ok(newReview.Id>0, "Review added successfully.");
                }
                return Result<bool>.Fail("SAVE_FAILED", "Could not save the review.");
            }
            catch (ArgumentException ex)
            {
                return Result<bool>.Fail("INVALID_DATA", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Result<bool>.Fail("BUSINESS_RULE_VIOLATION", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

     
        public async Task<Result<bool>> Delete(int id)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var review = await _unitOfWork.reviewRepository.GetByIdAsync(id);
                if (review == null)
                {
                    return Result<bool>.Fail("NOT_FOUND", "Review not found.");
                }

                if (review.ReviewerId != currentUserId)
                {
                   
                    return Result<bool>.Fail("FORBIDDEN", "You do not have permission to delete this review.");
                }

               await _unitOfWork.reviewRepository.Delete(review.Id); 
                int success = await _unitOfWork.CompleteTask();

                if (success > 0)
                {
                    return Result<bool>.Ok(true, "Review deleted successfully.");
                }
                return Result<bool>.Fail("DELETE_FAILED", "Could not delete the review.");

            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

      


        public async Task<Result<bool>> Update(int id, UpdateReviewDto updateReviewDto)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null)
                {
                    return Result<bool>.Fail("UNAUTHORIZED", "User is not authenticated.");
                }

                var review = await _unitOfWork.reviewRepository.GetByIdAsync(id); 
                if (review == null)
                {
                    return Result<bool>.Fail("NOT_FOUND", "Review not found.");
                }

                if (review.ReviewerId != currentUserId)
                {
                    return Result<bool>.Fail("FORBIDDEN", "You do not have permission to update this review.");
                }

                review.UpdateReview(updateReviewDto.SummaryReview, updateReviewDto.Rating);

                _unitOfWork.reviewRepository.Update(review);
                int success = await _unitOfWork.CompleteTask();

                if (success > 0)
                {
                    return Result<bool>.Ok(true, "Review updated successfully.");
                }
                return Result<bool>.Fail("UPDATE_FAILED", "Could not update the review.");
            }
            catch (ArgumentException ex)
            {
                return Result<bool>.Fail("INVALID_DATA", ex.Message);
            }
            catch (InvalidOperationException ex) 
            {
                return Result<bool>.Fail("BUSINESS_RULE_VIOLATION", ex.Message);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }

        public async Task<Result<List<ReviewDto>>> GetReviewsByProductId(int productId)
        {
            try
            {
                var productExists = await _unitOfWork.Products.GetByIdAsync(productId);
                if (productExists == null || productExists.IsDeleted == true)
                {
                    return Result<List<ReviewDto>>.Fail("PRODUCT_NOT_FOUND", "Product not found or has been deleted.");
                }

                var reviews = await _unitOfWork.reviewRepository.GetReviewsByProductIdAsync(productId);
                if (reviews == null || !reviews.Any())
                {
                    return Result<List<ReviewDto>>.Ok(new List<ReviewDto>(), "No reviews found for this product.");
                    
                }

                var reviewDtos = _mappingServices.MapList<Review, ReviewDto>(reviews).ToList();
                return Result<List<ReviewDto>>.Ok(reviewDtos, "Reviews retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<ReviewDto>>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }


        public async Task<Result<List<ReviewDto>>> GetReviewsByUserId(int userId)
        {
            try
            {
                int? currentUserId = _currentUserService.GetCurrentUserId();
                if (currentUserId == null || currentUserId != userId)
                {
                    return Result<List<ReviewDto>>.Fail("UNAUTHORIZED", "You do not have permission to view these reviews.");
                }
                var reviews = await _unitOfWork.reviewRepository.GetReviewsByUserIdAsync(userId);
                if (reviews == null || !reviews.Any())
                {
                    return Result<List<ReviewDto>>.Ok(new List<ReviewDto>(), "No reviews found for this user.");
                }

                var reviewDtos = _mappingServices.MapList<Review, ReviewDto>(reviews).ToList();
                return Result<List<ReviewDto>>.Ok(reviewDtos, "User reviews retrieved successfully.");
            }
            catch (Exception ex)
            {
                return Result<List<ReviewDto>>.Fail("SERVER_ERROR", $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}