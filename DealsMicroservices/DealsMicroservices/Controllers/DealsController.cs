using Microsoft.AspNetCore.Mvc;
using DealsMicroservices.DTOs;

namespace DealsMicroservices.Controllers
{
    /// <summary>
    /// Controller for Deals operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DealsController : ControllerBase
    {
        private readonly DealsMicroservices.Services.IDealsService _dealsService;
        private readonly ILogger<DealsController> _logger;

        public DealsController(DealsMicroservices.Services.IDealsService dealsService, ILogger<DealsController> logger)
        {
            _dealsService = dealsService;
            _logger = logger;
        }

        /// <summary>
        /// Get paginated deals listing
        /// </summary>
        /// <param name="request">Deals listing request parameters</param>
        /// <returns>Paginated deals listing response</returns>
        [HttpGet("listing")]
        public async Task<ActionResult<DealsListingResponseDto>> GetDealsListing([FromQuery] DealsListingRequestDto request)
        {
            try
            {
                _logger.LogInformation("Getting deals listing for page {Page} with size {Size}",
                    request.Page, request.Size);

                var result = await _dealsService.GetDealsListingAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Invalid request parameters: {Message}", ex.Message);
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deals listing");
                return StatusCode(500, new { error = "Internal server error occurred while retrieving deals" });
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
                var isHealthy = await _dealsService.HealthCheckAsync();

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

        /// <summary>
        /// Get deals aggregated by country for world map visualization
        /// </summary>
        /// <returns>Deals aggregated by country with volume and value totals</returns>
        [HttpGet("by-country")]
        public async Task<ActionResult<DealsByCountryResponseDto>> GetDealsByCountry()
        {
            try
            {
                _logger.LogInformation("Getting deals aggregated by country");

                var result = await _dealsService.GetDealsByCountryAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deals by country");
                return StatusCode(500, new { error = "Internal server error occurred while retrieving deals by country" });
            }
        }

        /// <summary>
        /// Get deals aggregated by type for analytics visualization
        /// </summary>
        /// <returns>Deals aggregated by type with volume and value totals</returns>
        [HttpGet("by-type")]
        public async Task<ActionResult<DealsByTypeResponseDto>> GetDealsByType()
        {
            try
            {
                _logger.LogInformation("Getting deals aggregated by type");

                var result = await _dealsService.GetDealsByTypeAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deals by type");
                return StatusCode(500, new { error = "Internal server error occurred while retrieving deals by type" });
            }
        }

        /// <summary>
        /// Get deals aggregated by status for analytics visualization
        /// </summary>
        /// <returns>Deals aggregated by status with volume and value totals</returns>
        [HttpGet("by-status")]
        public async Task<ActionResult<DealsByStatusResponseDto>> GetDealsByStatus()
        {
            try
            {
                _logger.LogInformation("Getting deals aggregated by status");

                var result = await _dealsService.GetDealsByStatusAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting deals by status");
                return StatusCode(500, new { error = "Internal server error occurred while retrieving deals by status" });
            }
        }
    }
}
