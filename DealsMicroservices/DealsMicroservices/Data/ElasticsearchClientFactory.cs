using Nest;
using Microsoft.Extensions.Options;
using DealsMicroservices.Models;

namespace DealsMicroservices.Data
{
    /// <summary>
    /// Factory for creating Elasticsearch client instances
    /// </summary>
    public class ElasticsearchClientFactory
    {
        private readonly ElasticsearchSettings _settings;

        public ElasticsearchClientFactory(IOptions<ElasticsearchSettings> settings)
        {
            _settings = settings.Value;
        }
        public IElasticClient CreateClient()
        {
            var connectionSettings = new ConnectionSettings(new Uri(_settings.Uri))
                .DefaultIndex(_settings.IndexName)
                .DisableDirectStreaming(); // For debugging

            return new ElasticClient(connectionSettings);
        }
    }
}
