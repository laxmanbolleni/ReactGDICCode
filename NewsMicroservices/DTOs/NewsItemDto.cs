namespace NewsMicroservices.DTOs
{
    /// <summary>
    /// Data Transfer Object for News Item API response
    /// </summary>
    public class NewsItemDto
    {
        public int NewsArticleId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string UrlNode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> NewsEventTypes { get; set; } = [];
        public List<string> RelatedCompanyNames { get; set; } = [];
        public List<string> Locations { get; set; } = [];
        public string Sentiment { get; set; } = string.Empty;
    }
}
