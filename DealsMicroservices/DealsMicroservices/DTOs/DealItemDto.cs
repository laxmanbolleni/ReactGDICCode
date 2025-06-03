namespace DealsMicroservices.DTOs
{
    /// <summary>
    /// Data Transfer Object for Deal Item API response
    /// </summary>
    public class DealItemDto
    {
        public int BaseDealId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string UrlNode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string DealType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal DealValue { get; set; }
    }
}
