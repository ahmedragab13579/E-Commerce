using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.ProductsRecommendations
{
    public class RecommendationItemDto
    {
        [JsonPropertyName("RecommendedProductId")]
        public int RecommendedProductId { get; set; }

        [JsonPropertyName("Score")]
        public double Score { get; set; }
    }
}
