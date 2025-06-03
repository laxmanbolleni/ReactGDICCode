using Nest;
using Microsoft.Extensions.Options;
using NewsMicroservices.DTOs;
using NewsMicroservices.Models;

namespace NewsMicroservices.Repositories
{
    /// <summary>
    /// Implementation of News repository using Elasticsearch
    /// </summary>
    public class NewsRepository : INewsRepository
    {
        private readonly IElasticClient _elasticClient;
        private readonly ElasticsearchSettings _settings;
        private readonly ILogger<NewsRepository> _logger;
        private static readonly string[] SearchFields = ["title"];

        public NewsRepository(IElasticClient elasticClient, IOptions<ElasticsearchSettings> settings, ILogger<NewsRepository> logger)
        {
            _elasticClient = elasticClient;
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<NewsListingResponseDto> GetNewsListingAsync(NewsListingRequestDto request)
        {
            try
            {
                var searchRequest = new SearchRequest<ElasticsearchDocument>(_settings.IndexName, _settings.TypeName)
                {
                    From = (request.Page - 1) * request.Size,
                    Size = request.Size,
                    Query = BuildQuery(request),
                    Sort = [new SortField { Field = "publishedDate", Order = SortOrder.Descending }],
                    Source = new SourceFilter
                    {
                        Includes = new[]
                        {
                            "newsArticleId",
                            "publishedDate",
                            "urlNode",
                            "title",
                            "newsEventTypes",
                            "newsArticleCompanies",
                            "locations",
                            "sentiments"
                        }
                    }
                };

                var response = await _elasticClient.SearchAsync<ElasticsearchDocument>(searchRequest);

                if (!response.IsValid)
                {
                    _logger.LogError("Elasticsearch query failed: {Error}", response.OriginalException?.Message);
                    throw new Exception($"Elasticsearch query failed: {response.OriginalException?.Message}");
                }
                var items = response.Documents.Select(MapToNewsItemDto).ToList();
                var totalItems = (int)response.Total;
                var totalPages = (int)Math.Ceiling((double)totalItems / request.Size);

                return new NewsListingResponseDto
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
                _logger.LogError(ex, "Error retrieving news listing");
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

        private static QueryContainer BuildQuery(NewsListingRequestDto request)
        {
            var queries = new List<QueryContainer>();

            // Text search in title
            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                queries.Add(new MultiMatchQuery
                {
                    Query = request.Query,
                    Fields = SearchFields,
                    Type = TextQueryType.BestFields
                });
            }

            // Filter by categories
            if (request.Categories?.Count > 0)
            {
                queries.Add(new TermsQuery
                {
                    Field = "newsEventTypes",
                    Terms = request.Categories
                });
            }

            // Filter by companies
            if (request.Companies?.Count > 0)
            {
                queries.Add(new NestedQuery
                {
                    Path = "newsArticleCompanies",
                    Query = new TermsQuery
                    {
                        Field = "newsArticleCompanies.relatedCompanyName",
                        Terms = request.Companies
                    }
                });
            }

            // Filter by locations
            if (request.Locations?.Count > 0)
            {
                queries.Add(new TermsQuery
                {
                    Field = "locations",
                    Terms = request.Locations
                });
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
        private NewsItemDto MapToNewsItemDto(ElasticsearchDocument document)
        {
            return new NewsItemDto
            {
                NewsArticleId = document.NewsArticleId,
                PublishedDate = document.PublishedDate,
                UrlNode = document.UrlNode ?? string.Empty,
                Title = document.Title ?? string.Empty,
                NewsEventTypes = document.NewsEventTypes ?? [],
                RelatedCompanyNames = ExtractCompanyNames(document.NewsArticleCompanies),
                Locations = document.Locations ?? [],
                Sentiment = document.Sentiments ?? string.Empty
            };
        }

        private static List<string> ExtractCompanyNames(List<NewsCompany> companies)
        {
            var companyNames = new List<string>();

            foreach (var company in companies)
            {
                if (!string.IsNullOrWhiteSpace(company.RelatedCompanyName))
                {
                    companyNames.Add(company.RelatedCompanyName);
                }
            }

            return companyNames;
        }
    }
}
