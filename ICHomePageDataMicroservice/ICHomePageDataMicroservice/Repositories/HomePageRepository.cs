using ICHomePageDataMicroservice.Configuration;
using ICHomePageDataMicroservice.Models;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ICHomePageDataMicroservice.Repositories
{
    public class HomePageRepository : IHomePageRepository
    {
        private readonly DatabaseSettings _databaseSettings;
        private readonly ILogger<HomePageRepository> _logger;

        public HomePageRepository(IOptions<DatabaseSettings> databaseSettings, ILogger<HomePageRepository> logger)
        {
            _databaseSettings = databaseSettings.Value;
            _logger = logger;
        }

        public async Task<HomePageStatistics> GetHomePageStatisticsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching homepage statistics from database");

                // For demo purposes, return mock data
                // In production, this would query actual database tables
                var statistics = new HomePageStatistics
                {
                    TotalNewsArticles = 7700000,
                    TotalDeals = 2600000,
                    TotalReports = 91580,
                    TotalClinicalTrials = 282194,
                    TotalCompanies = 453516,
                    TotalInvestigators = 226032,
                    LastUpdated = DateTime.UtcNow
                };

                // For demo purposes, simulate database operation
                // In production, this would query actual database tables
                await Task.Delay(10); // Simulate database query time
                _logger.LogInformation("Using mock data for statistics");

                _logger.LogInformation("Successfully retrieved homepage statistics");
                return statistics;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching homepage statistics");
                throw;
            }
        }

        public async Task<List<FeaturedContentItem>> GetFeaturedContentAsync(int limit = 10)
        {
            try
            {
                _logger.LogInformation($"Fetching featured content with limit: {limit}");

                // Mock featured content data
                var featuredContent = new List<FeaturedContentItem>
                {
                    new FeaturedContentItem
                    {
                        Id = 1,
                        Title = "Global Pharmaceutical Market Analysis 2024",
                        Description = "Comprehensive analysis of the global pharmaceutical market trends and forecasts",
                        ContentType = "Report",
                        Url = "/reports/pharma-market-2024",
                        PublishedDate = DateTime.UtcNow.AddDays(-2),
                        Category = "Pharmaceuticals",
                        Priority = 1,
                        IsActive = true
                    },
                    new FeaturedContentItem
                    {
                        Id = 2,
                        Title = "Major M&A Deal in Biotech Sector",
                        Description = "Analysis of the recent $5.2B acquisition in the biotechnology sector",
                        ContentType = "Deal",
                        Url = "/deals/biotech-ma-2024",
                        PublishedDate = DateTime.UtcNow.AddDays(-1),
                        Category = "Biotechnology",
                        Priority = 2,
                        IsActive = true
                    },
                    new FeaturedContentItem
                    {
                        Id = 3,
                        Title = "COVID-19 Vaccine Development Updates",
                        Description = "Latest developments in COVID-19 vaccine research and clinical trials",
                        ContentType = "News",
                        Url = "/news/covid-vaccine-updates",
                        PublishedDate = DateTime.UtcNow.AddHours(-6),
                        Category = "Healthcare",
                        Priority = 3,
                        IsActive = true
                    }
                };

                await Task.Delay(50); // Simulate async operation

                _logger.LogInformation($"Successfully retrieved {featuredContent.Count} featured content items");
                return featuredContent.Take(limit).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching featured content");
                throw;
            }
        }

        public async Task<List<RecentActivityItem>> GetRecentActivitiesAsync(int limit = 20)
        {
            try
            {
                _logger.LogInformation($"Fetching recent activities with limit: {limit}");

                // Mock recent activities data
                var recentActivities = new List<RecentActivityItem>
                {
                    new RecentActivityItem
                    {
                        Id = 1,
                        ActivityType = "News",
                        Title = "FDA Approves New Cancer Treatment",
                        Description = "Revolutionary cancer treatment receives FDA approval after successful Phase III trials",
                        ActivityDate = DateTime.UtcNow.AddMinutes(-30),
                        Source = "FDA Press Release",
                        Url = "/news/fda-cancer-approval"
                    },
                    new RecentActivityItem
                    {
                        Id = 2,
                        ActivityType = "Deal",
                        Title = "Venture Capital Investment in AI Healthcare",
                        Description = "$50M Series B funding for AI-powered diagnostic platform",
                        ActivityDate = DateTime.UtcNow.AddHours(-2),
                        Source = "Investment News",
                        Url = "/deals/ai-healthcare-funding"
                    },
                    new RecentActivityItem
                    {
                        Id = 3,
                        ActivityType = "Report",
                        Title = "Quarterly Market Intelligence Report",
                        Description = "Q4 2024 market intelligence report now available",
                        ActivityDate = DateTime.UtcNow.AddHours(-4),
                        Source = "GlobalData Research",
                        Url = "/reports/q4-2024-intelligence"
                    }
                };

                await Task.Delay(30); // Simulate async operation

                _logger.LogInformation($"Successfully retrieved {recentActivities.Count} recent activities");
                return recentActivities.Take(limit).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching recent activities");
                throw;
            }
        }

        public async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                using var connection = new SqlConnection(_databaseSettings.ConnectionString);
                await connection.OpenAsync();
                await connection.CloseAsync();
                _logger.LogInformation("Database connection test successful");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection test failed");
                return false;
            }
        }

        public async Task<List<HomePageCarouselItem>> GetHomePageCarouselAsync(string siteName, string userType)
        {
            try
            {
                _logger.LogInformation("Fetching homepage carousel using ICConsumerHomePageCarousel stored procedure");
                _logger.LogInformation($"Using connection string: {_databaseSettings.ConnectionString.Substring(0, Math.Min(50, _databaseSettings.ConnectionString.Length))}...");

                var carouselItems = new List<HomePageCarouselItem>();

                using var connection = new SqlConnection(_databaseSettings.ConnectionString);

                _logger.LogInformation("Opening database connection...");
                await connection.OpenAsync();
                _logger.LogInformation("Database connection opened successfully");

                using var command = new SqlCommand("ICConsumerHomePageCarousel", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 30
                };

                // No parameters needed for ICConsumerHomePageCarousel

                _logger.LogInformation("Executing stored procedure ICConsumerHomePageCarousel (no parameters)");

                using var reader = await command.ExecuteReaderAsync();

                // Check if we have any columns returned
                if (reader.FieldCount == 0)
                {
                    _logger.LogWarning("Stored procedure returned no columns");
                    return carouselItems;
                }

                _logger.LogInformation($"Stored procedure returned {reader.FieldCount} columns");

                while (await reader.ReadAsync())
                {
                    try
                    {
                        var item = new HomePageCarouselItem();

                        // Map fields from ICConsumerHomePageCarousel stored procedure
                        item.CarouselId = reader.HasColumn("CarouselId") ? reader.GetInt32("CarouselId") : 0;
                        item.CarouselName = reader.HasColumn("CarouselName") && !reader.IsDBNull("CarouselName") ? reader.GetString("CarouselName") : string.Empty;
                        item.CarouselImageURL = reader.HasColumn("CarouselImageURL") && !reader.IsDBNull("CarouselImageURL") ? reader.GetString("CarouselImageURL") : string.Empty;
                        item.CarouselLinkText = reader.HasColumn("CarouselLinkText") && !reader.IsDBNull("CarouselLinkText") ? reader.GetString("CarouselLinkText") : string.Empty;
                        item.CarouselURL = reader.HasColumn("CarouselURL") && !reader.IsDBNull("CarouselURL") ? reader.GetString("CarouselURL") : string.Empty;
                        item.CarouselDescription = reader.HasColumn("CarouselDescription") && !reader.IsDBNull("CarouselDescription") ? reader.GetString("CarouselDescription") : string.Empty;
                        item.CarouselSequence = reader.HasColumn("CarouselSequence") ? reader.GetInt32("CarouselSequence") : 0;

                        carouselItems.Add(item);
                    }
                    catch (Exception rowEx)
                    {
                        _logger.LogError(rowEx, "Error reading carousel item row");
                        // Continue processing other rows
                    }
                }

                _logger.LogInformation($"Successfully retrieved {carouselItems.Count} carousel items");
                return carouselItems;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, $"SQL Error executing ICConsumerHomePageCarousel stored procedure. Error Number: {sqlEx.Number}, Severity: {sqlEx.Class}, State: {sqlEx.State}");
                throw new Exception($"Database error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error executing ICConsumerHomePageCarousel stored procedure");
                throw;
            }
        }

        public async Task<List<HomePageKeyStats>> GetHomePageKeyStatsAsync(string siteName)
        {
            try
            {
                _logger.LogInformation($"Fetching homepage key stats for site: {siteName}");

                var keyStats = new List<HomePageKeyStats>();

                using var connection = new SqlConnection(_databaseSettings.ConnectionString);

                _logger.LogInformation("Opening database connection for key stats...");
                await connection.OpenAsync();
                _logger.LogInformation("Database connection opened successfully for key stats");

                using var command = new SqlCommand("GetConsumerHomePageKeyStats", connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 30
                };

                command.Parameters.AddWithValue("@SiteName", siteName);

                _logger.LogInformation($"Executing stored procedure GetConsumerHomePageKeyStats with parameter: SiteName='{siteName}'");

                using var reader = await command.ExecuteReaderAsync();

                if (reader.FieldCount == 0)
                {
                    _logger.LogWarning("GetConsumerHomePageKeyStats stored procedure returned no columns");
                    return keyStats;
                }

                _logger.LogInformation($"GetConsumerHomePageKeyStats returned {reader.FieldCount} columns");

                while (await reader.ReadAsync())
                {
                    try
                    {
                        var stat = new HomePageKeyStats
                        {
                            Name = reader.HasColumn("Name") && !reader.IsDBNull("Name") ? reader.GetString("Name") : string.Empty,
                            Value = reader.HasColumn("Value") ? reader.GetInt64("Value") : 0
                        };
                        keyStats.Add(stat);
                    }
                    catch (Exception rowEx)
                    {
                        _logger.LogError(rowEx, "Error reading key stats row");
                        // Continue processing other rows
                    }
                }

                _logger.LogInformation($"Successfully retrieved {keyStats.Count} key statistics");
                return keyStats;
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, $"SQL Error executing GetConsumerHomePageKeyStats for site: {siteName}. Error Number: {sqlEx.Number}, Severity: {sqlEx.Class}, State: {sqlEx.State}");
                throw new Exception($"Database error: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"General error executing GetConsumerHomePageKeyStats for site: {siteName}");
                throw;
            }
        }
    }

    // Extension method to safely check if a column exists
    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            try
            {
                return reader.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}
