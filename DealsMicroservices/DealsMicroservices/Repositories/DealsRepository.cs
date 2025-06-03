using Nest;
using Microsoft.Extensions.Options;
using DealsMicroservices.DTOs;
using DealsMicroservices.Models;
using DealsMicroservices.Data;

namespace DealsMicroservices.Repositories
{
    /// <summary>
    /// Implementation of Deals repository using Elasticsearch
    /// </summary>
    public class DealsRepository : IDealsRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticsearchSettings _settings;
        private readonly ILogger<DealsRepository> _logger;

        public DealsRepository(ElasticsearchClientFactory clientFactory, IOptions<ElasticsearchSettings> settings, ILogger<DealsRepository> logger)
        {
            _elasticClient = clientFactory.CreateClient();
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<DealsListingResponseDto> GetDealsListingAsync(DealsListingRequestDto request)
        {
            try
            {
                var searchRequest = new SearchRequest<ElasticsearchDocument>(_settings.IndexName, _settings.TypeName)
                {
                    Query = BuildQuery(request),
                    Size = request.Size,
                    From = (request.Page - 1) * request.Size,
                    Sort = [new SortField { Field = "publishedDate", Order = SortOrder.Descending }],
                    Source = new SourceFilter
                    {
                        Includes = new[]
                        {
                            "baseDealId",
                            "publishedDate",
                            "urlNode",
                            "title",
                            "dealCountryvalue",
                            "dealType",
                            "dealStatus",
                            "dealValue"
                        }
                    }
                };

                var response = await _elasticClient.SearchAsync<ElasticsearchDocument>(searchRequest);

                if (!response.IsValid)
                {
                    _logger.LogError("Elasticsearch search failed: {Error}", response.OriginalException?.Message ?? response.ServerError?.ToString());
                    throw new Exception($"Elasticsearch search failed: {response.OriginalException?.Message ?? response.ServerError?.ToString()}");
                }

                var items = response.Documents.Select(MapToDealItemDto).ToList();
                var totalItems = (int)response.Total;
                var totalPages = (int)Math.Ceiling((double)totalItems / request.Size);

                return new DealsListingResponseDto
                {
                    Items = items,
                    PageNumber = request.Page,
                    PageSize = request.Size,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    HasNextPage = request.Page < totalPages,
                    HasPreviousPage = request.Page > 1
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving deals listing");
                throw;
            }
        }
        public async Task<DealsByCountryResponseDto> GetDealsByCountryAsync()
        {
            try
            {
                var searchResponse = await _elasticClient.SearchAsync<ElasticsearchDocument>(s => s
                    .Index(_settings.IndexName)
                    .Type(_settings.TypeName)
                    .Size(0) // We only want aggregations, not documents
                    .Query(q => q.MatchAll())
                    .Aggregations(a => a.Terms("DealsByCountry", t => t
                            .Field("dealCountryvalue.keyword")
                            .Size(250)
                            .Aggregations(aa => aa
                                .Sum("DealValue", sum => sum.Field("dealValue"))
                                .Cardinality("Dealvolume", card => card.Field("baseDealId"))
                            )
                        )
                    )
                );

                if (!searchResponse.IsValid)
                {
                    var errorMessage = searchResponse.OriginalException?.Message ??
                                      searchResponse.ServerError?.Error?.Reason ??
                                      "Unknown Elasticsearch error";
                    _logger.LogError("Elasticsearch query failed: {Error}. Debug info: {DebugInfo}",
                                    errorMessage, searchResponse.DebugInformation);
                    throw new Exception($"Failed to execute deals by country aggregation: {errorMessage}");
                }

                var result = new DealsByCountryResponseDto(); _logger.LogInformation("Elasticsearch response is valid: {IsValid}, Aggregations count: {Count}",
                                     searchResponse.IsValid, searchResponse.Aggregations?.Count ?? 0);

                if (searchResponse.Aggregations?.TryGetValue("DealsByCountry", out var aggregation) == true)
                {
                    _logger.LogInformation("Found DealsByCountry aggregation, type: {Type}", aggregation.GetType().Name);

                    if (aggregation is BucketAggregate bucketAggregate)
                    {
                        _logger.LogInformation("BucketAggregate found with {Count} items", bucketAggregate.Items.Count);
                        foreach (var bucket in bucketAggregate.Items.OfType<KeyedBucket<object>>())
                        {
                            var country = bucket.Key?.ToString() ?? "Unknown";
                            var dealValue = 0m;
                            var dealVolume = 0L;

                            _logger.LogInformation("Processing bucket for country: {Country}, doc count: {DocCount}",
                                                 country, bucket.DocCount);

                            // Extract DealValue from nested aggregation
                            if (bucket.Aggregations.TryGetValue("DealValue", out var dealValueAgg) &&
                                dealValueAgg is ValueAggregate valueAggregate)
                            {
                                dealValue = (decimal)(valueAggregate.Value ?? 0);
                            }

                            // Extract Dealvolume from nested aggregation  
                            if (bucket.Aggregations.TryGetValue("Dealvolume", out var dealVolumeAgg) &&
                                dealVolumeAgg is ValueAggregate volumeAggregate)
                            {
                                dealVolume = (long)(volumeAggregate.Value ?? 0);
                            }

                            result.Countries.Add(new CountryDealAggregationDto
                            {
                                Country = country,
                                DealValue = dealValue,
                                DealVolume = dealVolume
                            });
                        }
                    }
                    else
                    {
                        _logger.LogWarning("DealsByCountry aggregation is not a BucketAggregate, it's: {Type}",
                                         aggregation.GetType().Name);
                    }
                }
                else
                {
                    _logger.LogWarning("DealsByCountry aggregation not found in response");
                }

                _logger.LogInformation("Successfully retrieved deals aggregation for {Count} countries", result.Countries.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing deals by country aggregation");
                throw;
            }
        }

        public async Task<DealsByTypeResponseDto> GetDealsByTypeAsync()
        {
            try
            {
                var searchResponse = await _elasticClient.SearchAsync<ElasticsearchDocument>(s => s
                    .Index(_settings.IndexName)
                    .Type(_settings.TypeName)
                    .Size(0) // We only want aggregations, not documents
                    .Query(q => q.MatchAll())
                    .Aggregations(a => a.Terms("DealsByType", t => t
                            .Field("dealType.keyword")
                            .Size(100)
                            .Aggregations(aa => aa
                                .Sum("DealValue", sum => sum.Field("dealValue"))
                                .Cardinality("Dealvolume", card => card.Field("baseDealId"))
                            )
                        )
                    )
                );

                if (!searchResponse.IsValid)
                {
                    var errorMessage = searchResponse.OriginalException?.Message ??
                                      searchResponse.ServerError?.Error?.Reason ??
                                      "Unknown Elasticsearch error";
                    _logger.LogError("Elasticsearch query failed: {Error}. Debug info: {DebugInfo}",
                                    errorMessage, searchResponse.DebugInformation);
                    throw new Exception($"Failed to execute deals by type aggregation: {errorMessage}");
                }

                var result = new DealsByTypeResponseDto();

                _logger.LogInformation("Elasticsearch response is valid: {IsValid}, Aggregations count: {Count}",
                                     searchResponse.IsValid, searchResponse.Aggregations?.Count ?? 0);

                if (searchResponse.Aggregations?.TryGetValue("DealsByType", out var aggregation) == true)
                {
                    _logger.LogInformation("Found DealsByType aggregation, type: {Type}", aggregation.GetType().Name);

                    if (aggregation is BucketAggregate bucketAggregate)
                    {
                        _logger.LogInformation("BucketAggregate found with {Count} items", bucketAggregate.Items.Count);
                        foreach (var bucket in bucketAggregate.Items.OfType<KeyedBucket<object>>())
                        {
                            var type = bucket.Key?.ToString() ?? "Unknown";
                            var dealValue = 0m;
                            var dealVolume = 0L;

                            _logger.LogInformation("Processing bucket for type: {Type}, doc count: {DocCount}",
                                                 type, bucket.DocCount);

                            // Extract DealValue from nested aggregation
                            if (bucket.Aggregations.TryGetValue("DealValue", out var dealValueAgg) &&
                                dealValueAgg is ValueAggregate valueAggregate)
                            {
                                dealValue = (decimal)(valueAggregate.Value ?? 0);
                            }

                            // Extract Dealvolume from nested aggregation  
                            if (bucket.Aggregations.TryGetValue("Dealvolume", out var dealVolumeAgg) &&
                                dealVolumeAgg is ValueAggregate volumeAggregate)
                            {
                                dealVolume = (long)(volumeAggregate.Value ?? 0);
                            }

                            result.Types.Add(new TypeDealAggregationDto
                            {
                                Type = type,
                                DealValue = dealValue,
                                DealVolume = dealVolume
                            });
                        }
                    }
                    else
                    {
                        _logger.LogWarning("DealsByType aggregation is not a BucketAggregate, it's: {Type}",
                                         aggregation.GetType().Name);
                    }
                }
                else
                {
                    _logger.LogWarning("DealsByType aggregation not found in response");
                }

                _logger.LogInformation("Successfully retrieved deals aggregation for {Count} types", result.Types.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing deals by type aggregation");
                throw;
            }
        }

        public async Task<DealsByStatusResponseDto> GetDealsByStatusAsync()
        {
            try
            {
                var searchResponse = await _elasticClient.SearchAsync<ElasticsearchDocument>(s => s
                    .Index(_settings.IndexName)
                    .Type(_settings.TypeName)
                    .Size(0) // We only want aggregations, not documents
                    .Query(q => q.MatchAll())
                    .Aggregations(a => a.Terms("DealsByStatus", t => t
                            .Field("dealStatus.keyword")
                            .Size(50)
                            .Aggregations(aa => aa
                                .Sum("DealValue", sum => sum.Field("dealValue"))
                                .Cardinality("Dealvolume", card => card.Field("baseDealId"))
                            )
                        )
                    )
                );

                if (!searchResponse.IsValid)
                {
                    var errorMessage = searchResponse.OriginalException?.Message ??
                                      searchResponse.ServerError?.Error?.Reason ??
                                      "Unknown Elasticsearch error";
                    _logger.LogError("Elasticsearch query failed: {Error}. Debug info: {DebugInfo}",
                                    errorMessage, searchResponse.DebugInformation);
                    throw new Exception($"Failed to execute deals by status aggregation: {errorMessage}");
                }

                var result = new DealsByStatusResponseDto();

                _logger.LogInformation("Elasticsearch response is valid: {IsValid}, Aggregations count: {Count}",
                                     searchResponse.IsValid, searchResponse.Aggregations?.Count ?? 0);

                if (searchResponse.Aggregations?.TryGetValue("DealsByStatus", out var aggregation) == true)
                {
                    _logger.LogInformation("Found DealsByStatus aggregation, type: {Type}", aggregation.GetType().Name);

                    if (aggregation is BucketAggregate bucketAggregate)
                    {
                        _logger.LogInformation("BucketAggregate found with {Count} items", bucketAggregate.Items.Count);
                        foreach (var bucket in bucketAggregate.Items.OfType<KeyedBucket<object>>())
                        {
                            var status = bucket.Key?.ToString() ?? "Unknown";
                            var dealValue = 0m;
                            var dealVolume = 0L;

                            _logger.LogInformation("Processing bucket for status: {Status}, doc count: {DocCount}",
                                                 status, bucket.DocCount);

                            // Extract DealValue from nested aggregation
                            if (bucket.Aggregations.TryGetValue("DealValue", out var dealValueAgg) &&
                                dealValueAgg is ValueAggregate valueAggregate)
                            {
                                dealValue = (decimal)(valueAggregate.Value ?? 0);
                            }

                            // Extract Dealvolume from nested aggregation  
                            if (bucket.Aggregations.TryGetValue("Dealvolume", out var dealVolumeAgg) &&
                                dealVolumeAgg is ValueAggregate volumeAggregate)
                            {
                                dealVolume = (long)(volumeAggregate.Value ?? 0);
                            }

                            result.Statuses.Add(new StatusDealAggregationDto
                            {
                                Status = status,
                                DealValue = dealValue,
                                DealVolume = dealVolume
                            });
                        }
                    }
                    else
                    {
                        _logger.LogWarning("DealsByStatus aggregation is not a BucketAggregate, it's: {Type}",
                                         aggregation.GetType().Name);
                    }
                }
                else
                {
                    _logger.LogWarning("DealsByStatus aggregation not found in response");
                }

                _logger.LogInformation("Successfully retrieved deals aggregation for {Count} statuses", result.Statuses.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing deals by status aggregation");
                throw;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _elasticClient.PingAsync();
                return response.IsValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error testing Elasticsearch connection");
                return false;
            }
        }

        private static QueryContainer BuildQuery(DealsListingRequestDto request)
        {
            var queries = new List<QueryContainer>();

            // Text search across title and other text fields
            if (!string.IsNullOrEmpty(request.Query))
            {
                var multiMatchQuery = new MultiMatchQuery
                {
                    Query = request.Query,
                    Fields = new[] { "title", "urlNode" }
                };
                queries.Add(multiMatchQuery);
            }

            // Filter by country
            if (!string.IsNullOrEmpty(request.Country))
            {
                var countryQuery = new TermQuery
                {
                    Field = "dealCountryvalue.keyword",
                    Value = request.Country
                };
                queries.Add(countryQuery);
            }

            // Filter by deal type
            if (!string.IsNullOrEmpty(request.DealType))
            {
                var dealTypeQuery = new TermQuery
                {
                    Field = "dealType.keyword",
                    Value = request.DealType
                };
                queries.Add(dealTypeQuery);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(request.Status))
            {
                var statusQuery = new TermQuery
                {
                    Field = "dealStatus.keyword",
                    Value = request.Status
                };
                queries.Add(statusQuery);
            }

            // Deal value range filter
            if (request.MinDealValue.HasValue || request.MaxDealValue.HasValue)
            {
                var dealValueRangeQuery = new NumericRangeQuery
                {
                    Field = "dealValue"
                };

                if (request.MinDealValue.HasValue)
                    dealValueRangeQuery.GreaterThanOrEqualTo = (double)request.MinDealValue.Value;

                if (request.MaxDealValue.HasValue)
                    dealValueRangeQuery.LessThanOrEqualTo = (double)request.MaxDealValue.Value;

                queries.Add(dealValueRangeQuery);
            }

            // Date range filter
            if (request.FromDate.HasValue || request.ToDate.HasValue)
            {
                var dateRangeQuery = new DateRangeQuery
                {
                    Field = "publishedDate"
                };

                if (request.FromDate.HasValue)
                    dateRangeQuery.GreaterThanOrEqualTo = request.FromDate.Value;

                if (request.ToDate.HasValue)
                    dateRangeQuery.LessThanOrEqualTo = request.ToDate.Value;

                queries.Add(dateRangeQuery);
            }

            return queries.Count > 0 ? new BoolQuery { Must = queries } : new MatchAllQuery();
        }
        private DealItemDto MapToDealItemDto(ElasticsearchDocument document)
        {
            return new DealItemDto
            {
                BaseDealId = document.BaseDealId,
                PublishedDate = document.PublishedDate,
                UrlNode = document.UrlNode ?? string.Empty,
                Title = document.Title ?? string.Empty,
                Country = document.DealCountryValue ?? string.Empty,
                DealType = document.DealType ?? string.Empty,
                Status = document.DealStatus ?? string.Empty,
                DealValue = document.DealValue
            };
        }
    }
}
