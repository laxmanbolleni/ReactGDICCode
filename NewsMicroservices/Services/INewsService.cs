using NewsMicroservices.DTOs;

namespace NewsMicroservices.Services
{
    /// <summary>
    /// Interface for News service operations
    /// </summary>
    public interface INewsService
    {
        Task<NewsListingResponseDto> GetNewsListingAsync(NewsListingRequestDto request);
        Task<bool> HealthCheckAsync();
    }
}
