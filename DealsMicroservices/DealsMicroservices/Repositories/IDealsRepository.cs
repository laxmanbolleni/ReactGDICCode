using DealsMicroservices.DTOs;

namespace DealsMicroservices.Repositories
{
    /// <summary>
    /// Interface for Deals repository operations
    /// </summary>
    public interface IDealsRepository
    {
        Task<DealsListingResponseDto> GetDealsListingAsync(DealsListingRequestDto request);
        Task<DealsByCountryResponseDto> GetDealsByCountryAsync();
        Task<DealsByTypeResponseDto> GetDealsByTypeAsync();
        Task<DealsByStatusResponseDto> GetDealsByStatusAsync();
        Task<bool> TestConnectionAsync();
    }
}
