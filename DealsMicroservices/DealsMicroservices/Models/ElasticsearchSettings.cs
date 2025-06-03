namespace DealsMicroservices.Models
{
    /// <summary>
    /// Configuration settings for Elasticsearch connection
    /// </summary>
    public class ElasticsearchSettings
    {
        public string Uri { get; set; } = string.Empty;
        public string IndexName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }
}
