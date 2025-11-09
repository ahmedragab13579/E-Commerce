using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Review
{
    public class UpdateReviewDto
    {
        [Required]
        [MaxLength(1000)]
        public string SummaryReview { get; private set; }

        [Range(1, 5)]
        public int Rating { get; private set; }
    }
}
