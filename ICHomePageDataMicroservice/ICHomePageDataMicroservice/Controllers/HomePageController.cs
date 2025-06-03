using ICHomePageDataMicroservice.Models;
using ICHomePageDataMicroservice.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICHomePageDataMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomePageController : ControllerBase
    {
        private readonly IHomePageService _homePageService;
        private readonly ILogger<HomePageController> _logger;

        public HomePageController(IHomePageService homePageService, ILogger<HomePageController> logger)
        {
            _homePageService = homePageService;
            _logger = logger;
        }

        /// <summary>
        /// Get homepage statistics including counts for news, deals, reports, and clinical trials
        /// </summary>
        /// <returns>Homepage statistics</returns>
        [HttpGet("statistics")]
        public async Task<ActionResult<HomePageStatisticsResponse>> GetStatistics()
        {
            try
            {
                _logger.LogInformation("Received request for homepage statistics");

                var result = await _homePageService.GetHomePageStatisticsAsync();

                if (result.Status == "Error")
                {
                    _logger.LogWarning($"Statistics request failed: {result.Message}");
                    return StatusCode(500, result);
                }

                _logger.LogInformation("Successfully returned homepage statistics");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetStatistics endpoint");
                return StatusCode(500, new HomePageStatisticsResponse
                {
                    Status = "Error",
                    Message = "An unexpected error occurred while retrieving statistics"
                });
            }
        }

        /// <summary>
        /// Get featured content for the homepage
        /// </summary>
        /// <param name="limit">Maximum number of featured items to return (default: 10)</param>
        /// <returns>Featured content items</returns>
        [HttpGet("featured")]
        public async Task<ActionResult<FeaturedContentResponse>> GetFeaturedContent([FromQuery] int limit = 10)
        {
            try
            {
                _logger.LogInformation($"Received request for featured content with limit: {limit}");

                if (limit <= 0 || limit > 50)
                {
                    _logger.LogWarning($"Invalid limit parameter: {limit}");
                    return BadRequest(new FeaturedContentResponse
                    {
                        Status = "Error",
                        Message = "Limit must be between 1 and 50"
                    });
                }

                var result = await _homePageService.GetFeaturedContentAsync(limit);

                if (result.Status == "Error")
                {
                    _logger.LogWarning($"Featured content request failed: {result.Message}");
                    return StatusCode(500, result);
                }

                _logger.LogInformation($"Successfully returned {result.TotalCount} featured content items");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetFeaturedContent endpoint");
                return StatusCode(500, new FeaturedContentResponse
                {
                    Status = "Error",
                    Message = "An unexpected error occurred while retrieving featured content"
                });
            }
        }

        /// <summary>
        /// Get recent activities for the homepage
        /// </summary>
        /// <param name="limit">Maximum number of recent activities to return (default: 20)</param>
        /// <returns>Recent activity items</returns>
        [HttpGet("recent-activities")]
        public async Task<ActionResult<RecentActivityResponse>> GetRecentActivities([FromQuery] int limit = 20)
        {
            try
            {
                _logger.LogInformation($"Received request for recent activities with limit: {limit}");

                if (limit <= 0 || limit > 100)
                {
                    _logger.LogWarning($"Invalid limit parameter: {limit}");
                    return BadRequest(new RecentActivityResponse
                    {
                        Status = "Error",
                        Message = "Limit must be between 1 and 100"
                    });
                }

                var result = await _homePageService.GetRecentActivitiesAsync(limit);

                if (result.Status == "Error")
                {
                    _logger.LogWarning($"Recent activities request failed: {result.Message}");
                    return StatusCode(500, result);
                }

                _logger.LogInformation($"Successfully returned {result.TotalCount} recent activities");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetRecentActivities endpoint");
                return StatusCode(500, new RecentActivityResponse
                {
                    Status = "Error",
                    Message = "An unexpected error occurred while retrieving recent activities"
                });
            }
        }

        /// <summary>
        /// Health check endpoint for monitoring
        /// </summary>
        /// <returns>Health status of the service</returns>
        [HttpGet("health")]
        public async Task<ActionResult<HealthCheckResponse>> GetHealth()
        {
            try
            {
                _logger.LogInformation("Received health check request");

                var result = await _homePageService.GetHealthStatusAsync();

                var statusCode = result.Status switch
                {
                    "Healthy" => 200,
                    "Degraded" => 200,
                    "Unhealthy" => 503,
                    _ => 503
                };

                _logger.LogInformation($"Health check completed with status: {result.Status}");
                return StatusCode(statusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in health check endpoint");
                return StatusCode(503, new HealthCheckResponse
                {
                    Status = "Unhealthy",
                    Service = "ICHomePageData",
                    Timestamp = DateTime.UtcNow,
                    Details = new Dictionary<string, object>
                    {
                        { "error", "Health check endpoint failed" }
                    }
                });
            }
        }

        /// <summary>
        /// Get homepage carousel items using stored procedure GetICHomePage
        /// </summary>
        /// <param name="siteName">Site name (default: "Canadean Intelligence Center")</param>
        /// <param name="userType">User type (default: "GlobalDataConsumer")</param>
        /// <returns>Homepage carousel items</returns>
        [HttpGet("carousel")]
        public async Task<ActionResult<HomePageCarouselResponse>> GetCarousel(
            [FromQuery] string siteName = "Canadean Intelligence Center",
            [FromQuery] string userType = "GlobalDataConsumer")
        {
            try
            {
                _logger.LogInformation($"Received request for homepage carousel - Site: {siteName}, User: {userType}");

                var result = await _homePageService.GetHomePageCarouselAsync(siteName, userType);

                if (result.Status == "Error")
                {
                    _logger.LogWarning($"Carousel request failed: {result.Message}");
                    return StatusCode(500, result);
                }

                _logger.LogInformation($"Successfully returned {result.TotalCount} carousel items");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetCarousel endpoint");
                return StatusCode(500, new HomePageCarouselResponse
                {
                    Status = "Error",
                    Message = "An unexpected error occurred while retrieving carousel items"
                });
            }
        }

        /// <summary>
        /// Get homepage key statistics using stored procedure GetHomePageKeyStats
        /// </summary>
        /// <param name="siteName">Site name (default: "Canadean Intelligence Center")</param>
        /// <returns>Homepage key statistics</returns>
        [HttpGet("key-stats")]
        public async Task<ActionResult<HomePageKeyStatsResponse>> GetKeyStats(
            [FromQuery] string siteName = "Canadean Intelligence Center")
        {
            try
            {
                _logger.LogInformation($"Received request for homepage key stats - Site: {siteName}");

                var result = await _homePageService.GetHomePageKeyStatsAsync(siteName);

                if (result.Status == "Error")
                {
                    _logger.LogWarning($"Key stats request failed: {result.Message}");
                    return StatusCode(500, result);
                }

                _logger.LogInformation($"Successfully returned {result.TotalCount} key statistics");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetKeyStats endpoint");
                return StatusCode(500, new HomePageKeyStatsResponse
                {
                    Status = "Error",
                    Message = "An unexpected error occurred while retrieving key statistics"
                });
            }
        }
    }
}
