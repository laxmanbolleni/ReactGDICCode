namespace DealsMicroservices.Models
{
    /// <summary>
    /// Domain model for Deal Item
    /// </summary>
    public class DealItem
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
