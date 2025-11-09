using Application.Dtos.ProductsRecommendations;
using Application.Results;
using Application.Services.InterFaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Infrastructure.Services.Implementaions
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RecommendationService> _logger; 

        public RecommendationService(IHttpClientFactory httpClientFactory, ILogger<RecommendationService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<Result<List<RecommendationItemDto>?>> GetRecommendationsAsync(int productId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RecommendationService");
                var response = await httpClient.GetAsync($"/recommendations/{productId}");

                if (response.IsSuccessStatusCode)
                {
                    var recommendationResponse = await response.Content
                        .ReadFromJsonAsync<RecommendationResponse>();
                    return Result<List<RecommendationItemDto>?>.Ok(recommendationResponse?.Recommendations, "api response successfully");
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogWarning($"No recommendations found for product {productId} (404).");
                    return Result<List<RecommendationItemDto>?>.Ok(null, "No recommendations found.");

                }

                _logger.LogError($"Error from recommendation service. Status: {response.StatusCode}");
                return Result<List<RecommendationItemDto>?>.Fail("NO_RETRIEVE","Failed to retrieve recommendations from the service.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to connect to recommendation service for product {productId}.");
                return Result<List<RecommendationItemDto>?>.Fail("CONNECTION_ERROR", "Error connecting to recommendation service.");
            }
        
        }
    }
}
