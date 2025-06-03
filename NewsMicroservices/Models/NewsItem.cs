namespace NewsMicroservices.Models
{
    /// <summary>
    /// Domain model for News Item
    /// </summary>
    public class NewsItem
    {
        public int NewsArticleId { get; set; }
        public DateTime PublishedDate { get; set; }
        public string UrlNode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = [];
        public List<string> Companies { get; set; } = [];
        public List<string> Geography { get; set; } = [];
        public string Sentiment { get; set; } = string.Empty;
    }
}
