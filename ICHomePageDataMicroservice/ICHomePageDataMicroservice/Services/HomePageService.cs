using ICHomePageDataMicroservice.Models;
using ICHomePageDataMicroservice.Repositories;

namespace ICHomePageDataMicroservice.Services
{
    public class HomePageService : IHomePageService
    {
        private readonly IHomePageRepository _repository;
        private readonly ILogger<HomePageService> _logger;

        public HomePageService(IHomePageRepository repository, ILogger<HomePageService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<HomePageStatisticsResponse> GetHomePageStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Processing request for homepage statistics");

                var statistics = await _repository.GetHomePageStatisticsAsync();

                var response = new HomePageStatisticsResponse
                {
                    TotalNewsArticles = statistics.TotalNewsArticles,
                    TotalDeals = statistics.TotalDeals,
                    TotalReports = statistics.TotalReports,
                    TotalClinicalTrials = statistics.TotalClinicalTrials,
                    TotalCompanies = statistics.TotalCompanies,
                    TotalInvestigators = statistics.TotalInvestigators,
                    LastUpdated = statistics.LastUpdated,
                    Status = "Success",
                    Message = "Statistics retrieved successfully"
                };

                _logger.LogInformation("Successfully processed homepage statistics request");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing homepage statistics request");
                return new HomePageStatisticsResponse
                {
                    Status = "Error",
                    Message = $"Failed to retrieve statistics: {ex.Message}"
                };
            }
        }

        public async Task<FeaturedContentResponse> GetFeaturedContentAsync(int limit = 10)
        {
            try
            {
                _logger.LogInformation($"Processing request for featured content with limit: {limit}");

                var items = await _repository.GetFeaturedContentAsync(limit);

                var response = new FeaturedContentResponse
                {
                    Items = items,
                    TotalCount = items.Count,
                    Status = "Success",
                    Message = "Featured content retrieved successfully"
                };

                _logger.LogInformation($"Successfully processed featured content request, returned {items.Count} items");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing featured content request");
                return new FeaturedContentResponse
                {
                    Status = "Error",
                    Message = $"Failed to retrieve featured content: {ex.Message}"
                };
            }
        }

        public async Task<RecentActivityResponse> GetRecentActivitiesAsync(int limit = 20)
        {
            try
            {
                _logger.LogInformation($"Processing request for recent activities with limit: {limit}");

                var activities = await _repository.GetRecentActivitiesAsync(limit);

                var response = new RecentActivityResponse
                {
                    Activities = activities,
                    TotalCount = activities.Count,
                    Status = "Success",
                    Message = "Recent activities retrieved successfully"
                };

                _logger.LogInformation($"Successfully processed recent activities request, returned {activities.Count} activities");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing recent activities request");
                return new RecentActivityResponse
                {
                    Status = "Error",
                    Message = $"Failed to retrieve recent activities: {ex.Message}"
                };
            }
        }

        public async Task<HealthCheckResponse> GetHealthStatusAsync()
        {
            try
            {
                _logger.LogInformation("Processing health check request");

                var dbConnectionStatus = await _repository.TestDatabaseConnectionAsync();

                var response = new HealthCheckResponse
                {
                    Status = dbConnectionStatus ? "Healthy" : "Degraded",
                    Service = "ICHomePageData",
                    Timestamp = DateTime.UtcNow,
                    Version = "1.0.0",
                    Details = new Dictionary<string, object>
                    {
                        { "database", dbConnectionStatus ? "Connected" : "Disconnected" },
                        { "uptime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC") },
                        { "environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production" }
                    }
                };

                _logger.LogInformation($"Health check completed with status: {response.Status}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during health check");
                return new HealthCheckResponse
                {
                    Status = "Unhealthy",
                    Service = "ICHomePageData",
                    Timestamp = DateTime.UtcNow,
                    Version = "1.0.0",
                    Details = new Dictionary<string, object>
                    {
                        { "error", ex.Message },
                        { "database", "Unknown" }
                    }
                };
            }
        }

        public async Task<HomePageCarouselResponse> GetHomePageCarouselAsync(string siteName = "Canadean Intelligence Center", string userType = "GlobalDataConsumer")
        {
            try
            {
                _logger.LogInformation($"Processing request for homepage carousel - Site: {siteName}, User: {userType}");

                var items = await _repository.GetHomePageCarouselAsync(siteName, userType);

                var response = new HomePageCarouselResponse
                {
                    Items = items,
                    TotalCount = items.Count,
                    Status = "Success",
                    Message = "Carousel items retrieved successfully"
                };

                _logger.LogInformation($"Successfully processed carousel request, returned {items.Count} items");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing carousel request for site: {siteName}, user: {userType}");
                return new HomePageCarouselResponse
                {
                    Status = "Error",
                    Message = $"Failed to retrieve carousel items: {ex.Message}"
                };
            }
        }

        public async Task<HomePageKeyStatsResponse> GetHomePageKeyStatsAsync(string siteName = "Canadean Intelligence Center")
        {
            try
            {
                _logger.LogInformation($"Processing request for homepage key stats - Site: {siteName}");

                var stats = await _repository.GetHomePageKeyStatsAsync(siteName);

                var response = new HomePageKeyStatsResponse
                {
                    Stats = stats,
                    TotalCount = stats.Count,
                    Status = "Success",
                    Message = "Key statistics retrieved successfully"
                };

                _logger.LogInformation($"Successfully processed key stats request, returned {stats.Count} statistics");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing key stats request for site: {siteName}");
                return new HomePageKeyStatsResponse
                {
                    Status = "Error",
                    Message = $"Failed to retrieve key statistics: {ex.Message}"
                };
            }
        }
    }
}
