using DealsMicroservices.DTOs;
using DealsMicroservices.Repositories;

namespace DealsMicroservices.Services
{
    /// <summary>
    /// Implementation of Deals service with business logic
    /// </summary>
    public class DealsService : IDealsService
    {
        private readonly IDealsRepository _dealsRepository;
        private readonly ILogger<DealsService> _logger;

        public DealsService(IDealsRepository dealsRepository, ILogger<DealsService> logger)
        {
            _dealsRepository = dealsRepository;
            _logger = logger;
        }

        public async Task<DealsListingResponseDto> GetDealsListingAsync(DealsListingRequestDto request)
        {
            try
            {
                // Validate request parameters
                ValidateRequest(request);

                // Get deals from repository
                var result = await _dealsRepository.GetDealsListingAsync(request);

                _logger.LogInformation("Retrieved {Count} deal items for page {Page}",
                    result.Items.Count, request.Page);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDealsListingAsync");
                throw;
            }
        }

        public async Task<DealsByCountryResponseDto> GetDealsByCountryAsync()
        {
            try
            {
                _logger.LogInformation("Getting deals aggregated by country");

                var result = await _dealsRepository.GetDealsByCountryAsync();

                _logger.LogInformation("Retrieved deals data for {Count} countries", result.Countries.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDealsByCountryAsync");
                throw;
            }
        }

        public async Task<DealsByTypeResponseDto> GetDealsByTypeAsync()
        {
            try
            {
                _logger.LogInformation("Getting deals aggregated by type");

                var result = await _dealsRepository.GetDealsByTypeAsync();

                _logger.LogInformation("Retrieved deals data for {Count} types", result.Types.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDealsByTypeAsync");
                throw;
            }
        }

        public async Task<DealsByStatusResponseDto> GetDealsByStatusAsync()
        {
            try
            {
                _logger.LogInformation("Getting deals aggregated by status");

                var result = await _dealsRepository.GetDealsByStatusAsync();

                _logger.LogInformation("Retrieved deals data for {Count} statuses", result.Statuses.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetDealsByStatusAsync");
                throw;
            }
        }

        public async Task<bool> HealthCheckAsync()
        {
            try
            {
                return await _dealsRepository.TestConnectionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return false;
            }
        }

        private static void ValidateRequest(DealsListingRequestDto request)
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

            // Validate deal value range
            if (request.MinDealValue.HasValue && request.MaxDealValue.HasValue)
            {
                if (request.MinDealValue > request.MaxDealValue)
                {
                    throw new ArgumentException("MinDealValue cannot be greater than MaxDealValue");
                }
            }
        }
    }
}
