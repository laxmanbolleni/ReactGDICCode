using NewsMicroservices.DTOs;
using NewsMicroservices.Repositories;

namespace NewsMicroservices.Services
{
    /// <summary>
    /// Implementation of News service with business logic
    /// </summary>
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;
        private readonly ILogger<NewsService> _logger;

        public NewsService(INewsRepository newsRepository, ILogger<NewsService> logger)
        {
            _newsRepository = newsRepository;
            _logger = logger;
        }

        public async Task<NewsListingResponseDto> GetNewsListingAsync(NewsListingRequestDto request)
        {
            try
            {
                // Validate request parameters
                ValidateRequest(request);

                // Get news from repository
                var result = await _newsRepository.GetNewsListingAsync(request);

                _logger.LogInformation("Retrieved {Count} news items for page {Page}",
                    result.Items.Count, request.Page);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetNewsListingAsync");
                throw;
            }
        }

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                return await _newsRepository.TestConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return false;
            }
        }

        private static void ValidateRequest(NewsListingRequestDto request)
        {
            if (request.Page < 1)
            {
                request.Page = 1;
            }

            if (request.Size < 1 || request.Size > 100)
            {
                request.Size = Math.Clamp(request.Size, 1, 100);
            }

            // Validate date range
            if (request.FromDate.HasValue && request.ToDate.HasValue)
            {
                if (request.FromDate > request.ToDate)
                {
                    throw new ArgumentException("FromDate cannot be greater than ToDate");
                }
            }
        }
    }
}
