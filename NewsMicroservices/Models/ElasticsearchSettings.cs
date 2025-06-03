namespace NewsMicroservices.Models
{
    /// <summary>
    /// Configuration settings for Elasticsearch connection
    /// </summary>
    public class ElasticsearchSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string IndexName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public int TimeoutInSeconds { get; set; } = 30;
    }
}
