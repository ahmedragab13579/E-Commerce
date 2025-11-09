using Application.Dtos.Product;
using Application.Dtos.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Review
{
    public class ReviewDto
    {
        public int Id { get; private set; }

        [Required]
        [MaxLength(1000)]
        public string SummaryReview { get; private set; }

        [Range(1, 5)]
        public int Rating { get; private set; }

        public DateTime DateSubmitted { get; private set; }

        public int ReviewerId { get; private set; }
        public UserDto Reviewer { get; private set; }

        public int ProductId { get; private set; }
        public ProductDto ReviewedProduct { get; private set; }

    }
}
