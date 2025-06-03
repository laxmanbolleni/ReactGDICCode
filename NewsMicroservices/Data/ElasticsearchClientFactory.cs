using Nest;
using Microsoft.Extensions.Options;
using NewsMicroservices.Models;

namespace NewsMicroservices.Data
{
    /// <summary>
    /// Factory for creating Elasticsearch client
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
            var uri = new Uri(_settings.ConnectionString);
            var connectionSettings = new ConnectionSettings(uri)
                .DefaultIndex(_settings.IndexName)
                .RequestTimeout(TimeSpan.FromSeconds(_settings.TimeoutInSeconds))
                .DisableDirectStreaming()
                .OnRequestCompleted(details =>
                {
                    // Log request details if needed
                    if (details.Success)
                    {
                        Console.WriteLine($"Elasticsearch request completed successfully: {details.HttpMethod} {details.Uri}");
                    }
                    else
                    {
                        Console.WriteLine($"Elasticsearch request failed: {details.HttpMethod} {details.Uri} - {details.OriginalException?.Message}");
                    }
                });

            return new ElasticClient(connectionSettings);
        }
    }
}
