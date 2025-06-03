using Newtonsoft.Json;

namespace DealsMicroservices.Models
{
    /// <summary>
    /// Elasticsearch document structure for the intelligencecenter/deals index
    /// </summary>
    public class ElasticsearchDocument
    {
        [JsonProperty("baseDealId")]
        public int BaseDealId { get; set; }

        [JsonProperty("publishedDate")]
        public DateTime PublishedDate { get; set; }

        [JsonProperty("urlNode")]
        public string UrlNode { get; set; } = string.Empty;

        [JsonProperty("title")]
        public string Title { get; set; } = string.Empty;

        [JsonProperty("dealCountryvalue")]
        public string DealCountryValue { get; set; } = string.Empty;

        [JsonProperty("dealType")]
        public string DealType { get; set; } = string.Empty;

        [JsonProperty("dealStatus")]
        public string DealStatus { get; set; } = string.Empty;

        [JsonProperty("dealValue")]
        public decimal DealValue { get; set; }
    }
}
