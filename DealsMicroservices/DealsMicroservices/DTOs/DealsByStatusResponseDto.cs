namespace DealsMicroservices.DTOs
{
    /// <summary>
    /// Data Transfer Object for Deals by Status aggregation response
    /// </summary>
    public class DealsByStatusResponseDto
    {
        public List<StatusDealAggregationDto> Statuses { get; set; } = [];
    }

    /// <summary>
    /// Data Transfer Object for individual deal status aggregation
    /// </summary>
    public class StatusDealAggregationDto
    {
        public string Status { get; set; } = string.Empty;
        public long DealVolume { get; set; }
        public decimal DealValue { get; set; }
    }
}
