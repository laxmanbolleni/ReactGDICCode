using ICHomePageDataMicroservice.Models;

namespace ICHomePageDataMicroservice.Services
{
    public interface IHomePageService
    {
        Task<HomePageStatisticsResponse> GetHomePageStatisticsAsync();
        Task<FeaturedContentResponse> GetFeaturedContentAsync(int limit = 10);
        Task<RecentActivityResponse> GetRecentActivitiesAsync(int limit = 20);
        Task<HealthCheckResponse> GetHealthStatusAsync();

        // New methods for stored procedures
        Task<HomePageCarouselResponse> GetHomePageCarouselAsync(string siteName = "Canadean Intelligence Center", string userType = "GlobalDataConsumer");
        Task<HomePageKeyStatsResponse> GetHomePageKeyStatsAsync(string siteName = "Canadean Intelligence Center");
    }
}
