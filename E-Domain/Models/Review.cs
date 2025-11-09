using System;
using System.ComponentModel.DataAnnotations; 

namespace E_Domain.Models
{
    public class Review
    {
        public int Id { get; private set; }

        [Required] 
        [MaxLength(1000)] 
        public string SummaryReview { get; private set; }

        [Range(1, 5)] 
        public int Rating { get; private set; }

        public DateTime DateSubmitted { get; private set; } 

        public int ReviewerId { get; private set; } 
        public User Reviewer { get; private set; }

        public int ProductId { get; private set; }
        public Product ReviewedProduct { get; private set; }

        private Review()
        {
        }

        public Review(string summaryReview, int rating, User reviewer, Product reviewedProduct)
        {
            if (string.IsNullOrWhiteSpace(summaryReview)) 
            {
                throw new ArgumentException("Review summary cannot be empty.", nameof(summaryReview));
            }
            if (rating < 1 || rating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
            }
            if (summaryReview.Length > 1000) 
            {
                throw new ArgumentException("Review summary cannot exceed 1000 characters.", nameof(summaryReview));
            }
            if (reviewedProduct == null)
            {
                throw new ArgumentNullException(nameof(reviewedProduct), "Reviewed product cannot be null.");
            }
            if (reviewer == null)
            {
                throw new ArgumentNullException(nameof(reviewer), "Reviewer cannot be null.");
            }

            

            if (reviewer.IsDeleted) 
            {
                throw new InvalidOperationException("Deleted users cannot submit reviews.");
            }
            if (reviewer.IsBlocked) 
            {
                throw new InvalidOperationException("Blocked users cannot submit reviews.");
            }

            SummaryReview = summaryReview;
            Rating = rating;
            ReviewerId = reviewer.Id; 
            Reviewer = reviewer;     
            ProductId = reviewedProduct.Id; 
            ReviewedProduct = reviewedProduct; 
            DateSubmitted = DateTime.UtcNow; 
        }

        public void UpdateReview(string newSummary, int newRating)
        {
            if (string.IsNullOrWhiteSpace(newSummary))
            {
                throw new ArgumentException("Review summary cannot be empty.", nameof(newSummary));
            }
            if (newRating < 1 || newRating > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(newRating), "Rating must be between 1 and 5.");
            }
            if (newSummary.Length > 1000)
            {
                throw new ArgumentException("Review summary cannot exceed 1000 characters.", nameof(newSummary));
            }

            SummaryReview = newSummary;
            Rating = newRating;
        }
    }
}