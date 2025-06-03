namespace DealsMicroservices.DTOs
{
    /// <summary>
    /// Data Transfer Object for Deals by Type aggregation response
    /// </summary>
    public class DealsByTypeResponseDto
    {
        public List<TypeDealAggregationDto> Types { get; set; } = [];
    }

    /// <summary>
    /// Data Transfer Object for individual deal type aggregation
    /// </summary>
    public class TypeDealAggregationDto
    {
        public string Type { get; set; } = string.Empty;
        public long DealVolume { get; set; }
        public decimal DealValue { get; set; }
    }
}
