using DealsMicroservices.DTOs;

namespace DealsMicroservices.Services
{
    /// <summary>
    /// Interface for Deals service operations
    /// </summary>
    public interface IDealsService
    {
        Task<DealsListingResponseDto> GetDealsListingAsync(DealsListingRequestDto request);
        Task<DealsByCountryResponseDto> GetDealsByCountryAsync();
        Task<DealsByTypeResponseDto> GetDealsByTypeAsync();
        Task<DealsByStatusResponseDto> GetDealsByStatusAsync();
        Task<bool> HealthCheckAsync();
    }
}
