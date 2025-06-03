using Newtonsoft.Json;

namespace NewsMicroservices.Models
{    /// <summary>
     /// Elasticsearch document structure for the intelligencecenter index
     /// </summary>
    public class ElasticsearchDocument
    {
        [JsonProperty("newsArticleId")]
        public int NewsArticleId { get; set; }

        [JsonProperty("publishedDate")]
        public DateTime PublishedDate { get; set; }

        [JsonProperty("urlNode")]
        public string UrlNode { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("newsEventTypes")]
        public List<string> NewsEventTypes { get; set; } = [];

        [JsonProperty("newsArticleCompanies")]
        public List<NewsCompany> NewsArticleCompanies { get; set; } = [];

        [JsonProperty("locations")]
        public List<string> Locations { get; set; } = [];

        [JsonProperty("sentiments")]
        public string Sentiments { get; set; } = string.Empty;
    }

    /// <summary>
    /// Company information from newsArticleCompanies
    /// </summary>
    public class NewsCompany
    {
        [JsonProperty("relatedCompanyName")]
        public string RelatedCompanyName { get; set; } = string.Empty;

        [JsonProperty("companyId")]
        public string CompanyId { get; set; } = string.Empty;
    }
}
