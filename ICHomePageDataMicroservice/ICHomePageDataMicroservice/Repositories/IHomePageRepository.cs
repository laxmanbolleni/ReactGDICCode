using ICHomePageDataMicroservice.Models;

namespace ICHomePageDataMicroservice.Repositories
{
    public interface IHomePageRepository
    {
        Task<HomePageStatistics> GetHomePageStatisticsAsync();
        Task<List<FeaturedContentItem>> GetFeaturedContentAsync(int limit = 10);
        Task<List<RecentActivityItem>> GetRecentActivitiesAsync(int limit = 20);
        Task<bool> TestDatabaseConnectionAsync();

        // New methods for stored procedures
        Task<List<HomePageCarouselItem>> GetHomePageCarouselAsync(string siteName, string userType);
        Task<List<HomePageKeyStats>> GetHomePageKeyStatsAsync(string siteName);
    }
}
