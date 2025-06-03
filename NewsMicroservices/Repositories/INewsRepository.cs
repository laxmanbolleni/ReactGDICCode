using NewsMicroservices.DTOs;

namespace NewsMicroservices.Repositories
{
    /// <summary>
    /// Interface for News repository operations
    /// </summary>
    public interface INewsRepository
    {
        Task<NewsListingResponseDto> GetNewsListingAsync(NewsListingRequestDto request);
        Task<bool> TestConnectionAsync();
    }
}
