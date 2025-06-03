namespace DealsMicroservices.DTOs
{
    /// <summary>
    /// Data Transfer Object for Deals by Country aggregation response
    /// </summary>
    public class DealsByCountryResponseDto
    {
        public List<CountryDealAggregationDto> Countries { get; set; } = [];
    }

    /// <summary>
    /// Data Transfer Object for individual country deal aggregation
    /// </summary>
    public class CountryDealAggregationDto
    {
        public string Country { get; set; } = string.Empty;
        public long DealVolume { get; set; }
        public decimal DealValue { get; set; }
    }
}
