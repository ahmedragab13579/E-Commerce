using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Dtos.ProductsRecommendations
{
    public class RecommendationResponse
    {
        [JsonPropertyName("product_id")]
        public int ProductId { get; set; }

        [JsonPropertyName("recommendations")]
        public List<RecommendationItemDto> Recommendations { get; set; }
    }
}
