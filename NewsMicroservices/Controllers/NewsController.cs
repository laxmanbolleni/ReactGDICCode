using Microsoft.AspNetCore.Mvc;
using NewsMicroservices.DTOs;
using NewsMicroservices.Services;

namespace NewsMicroservices.Controllers
{
    /// <summary>
    /// Controller for News operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ILogger<NewsController> _logger;

        public NewsController(INewsService newsService, ILogger<NewsController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated news listing
        /// </summary>
        /// <param name="request">News listing request parameters</param>
        /// <returns>Paginated news listing response</returns>
        [HttpGet("listing")]
        public async Task<ActionResult<NewsListingResponseDto>> GetNewsListing([FromQuery] NewsListingRequestDto request)
        {
            try
            {
                _logger.LogInformation("Getting news listing for page {Page} with size {Size}",
                    request.Page, request.Size);

                var result = await _newsService.GetNewsListingAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid request parameters: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting news listing");
                return StatusCode(500, new { error = "Internal server error occurred while retrieving news listing" });
            }
        }

        /// <summary>
        /// Health check endpoint
        /// </summary>
        /// <returns>Health status</returns>
        [HttpGet("health")]
        public async Task<ActionResult> HealthCheck()
        {
            try
            {
                var isHealthy = await _newsService.HealthCheckAsync();

                if (isHealthy)
                {
                    return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
                }
                else
                {
                    return StatusCode(503, new { status = "unhealthy", timestamp = DateTime.UtcNow });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(503, new { status = "unhealthy", error = ex.Message, timestamp = DateTime.UtcNow });
            }
        }
    }
}
