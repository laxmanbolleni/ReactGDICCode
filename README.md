# GlobalData Intelligence Center Microservices

## Overview

This repository contains two ASP.NET Core Web API microservices that integrate with Elasticsearch to provide access to news articles and deals data from the Intelligence Center.

## Architecture

Both microservices follow a clean architecture pattern with:

- **Controllers**: API endpoints and HTTP handling
- **Services**: Business logic layer
- **Repositories**: Data access layer for Elasticsearch
- **DTOs**: Data transfer objects for API communication
- **Models**: Domain models and Elasticsearch document models

## Microservices

### 1. NewsMicroservices

**Purpose**: Provides access to news articles from the Intelligence Center

**Base URL**: `http://localhost:5137`

**Elasticsearch Configuration**:

- **URL**: `http://iccloudbackup.elasticsearch.production.pdm.local:9200`
- **Index**: `intelligencecenter/newsarticle/_search`
- **NEST Version**: 5.6.1 (compatible with Elasticsearch 5.5.0)

**Endpoints**:

- `GET /api/news/health` - Health check endpoint
- `GET /api/news/listing` - Get paginated news articles

**Field Mappings**:

- `newsArticleId` → `newsArticleId` (int)
- `publishedDate` → `publishedDate` (date)
- `urlNode` → `urlNode` (string)
- `headline` → `headline` (string)
- `summary` → `summary` (string)
- `content` → `content` (string)
- `companies` → `companies` (List&lt;NewsCompany&gt;)

### 2. DealsMicroservices

**Purpose**: Provides access to deals data from the Intelligence Center

**Base URL**: `http://localhost:5215`

**Elasticsearch Configuration**:

- **URL**: `http://iccloudbackup.elasticsearch.production.pdm.local:9200`
- **Index**: `intelligencecenter/deals/_search`
- **NEST Version**: 5.6.1 (compatible with Elasticsearch 5.5.0)

**Endpoints**:

- `GET /api/deals/health` - Health check endpoint
- `GET /api/deals/listing` - Get paginated deals

**Field Mappings**:

- `baseDealId` → `baseDealId` (int)
- `publishedDate` → `date` (date)
- `urlNode` → `urlNode` (string)
- `title` → `headline` (string)
- `dealCountryvalue` → `Country` (string)
- `dealType` → `DealType` (string)
- `dealStatus` → `Status` (string)
- `dealValue` → `dealValue` (decimal)

## Common Features

### Query Parameters

Both microservices support the following common query parameters:

**Pagination**:

- `PageNumber`: Page number (default: 1)
- `PageSize`: Items per page (default: 10, max: 100)

**Date Filtering**:

- `DateFrom`: Filter items from this date (yyyy-MM-dd format)
- `DateTo`: Filter items to this date (yyyy-MM-dd format)

### DealsMicroservices Additional Filters

- `Country`: Filter by deal country
- `DealType`: Filter by deal type (e.g., "Merger", "Acquisition")
- `Status`: Filter by deal status (e.g., "Completed", "Pending")
- `MinDealValue`: Minimum deal value
- `MaxDealValue`: Maximum deal value

### NewsM‌icroservices Additional Filters

- `SearchTerm`: Search in headlines, summary, and content
- `Company`: Filter by company name

## Response Format

### Success Response

```json
{
    "items": [...],
    "pageNumber": 1,
    "pageSize": 10,
    "totalItems": 100,
    "totalPages": 10,
    "hasNextPage": true,
    "hasPreviousPage": false
}
```

### Health Check Response

```json
{
  "status": "healthy",
  "timestamp": "2025-05-31T13:11:13.6206161Z"
}
```

### Error Response

```json
{
  "error": "Error message",
  "details": "Additional error details"
}
```

## Setup and Installation

### Prerequisites

- .NET 9.0 SDK
- Access to Elasticsearch cluster at `iccloudbackup.elasticsearch.production.pdm.local:9200`

### Running the Services

**NewsMicroservices**:

```bash
cd c:\GDICCodebase\GlobalData.IC\NewsMicroservices
dotnet run
```

Service will start on `http://localhost:5137`

**DealsMicroservices**:

```bash
cd c:\GDICCodebase\GlobalData.IC\DealsMicroservices\DealsMicroservices
dotnet run --project DealsMicroservices.csproj
```

Service will start on `http://localhost:5215`

## Testing

### Manual Testing Examples

**Test Health Endpoints**:

```powershell
# News health check
Invoke-RestMethod -Uri "http://localhost:5137/api/news/health" -Method GET

# Deals health check
Invoke-RestMethod -Uri "http://localhost:5215/api/deals/health" -Method GET
```

**Test Listing Endpoints**:

```powershell
# Get news articles
Invoke-RestMethod -Uri "http://localhost:5137/api/news/listing" -Method GET

# Get deals
Invoke-RestMethod -Uri "http://localhost:5215/api/deals/listing" -Method GET
```

**Test with Filters**:

```powershell
# Get news with pagination
Invoke-RestMethod -Uri "http://localhost:5137/api/news/listing?PageSize=5&PageNumber=2" -Method GET

# Get deals by country and type
Invoke-RestMethod -Uri "http://localhost:5215/api/deals/listing?Country=USA&DealType=Merger" -Method GET

# Get deals by value range
Invoke-RestMethod -Uri "http://localhost:5215/api/deals/listing?MinDealValue=1000000&MaxDealValue=10000000" -Method GET
```

### Automated Testing Scripts

- **NewsMicroservices**: `test-news-api.ps1`
- **DealsMicroservices**: `test-deals-api.ps1`

Run the test scripts:

```powershell
# Test news API
.\test-news-api.ps1

# Test deals API
.\test-deals-api.ps1
```

## Configuration

### appsettings.json

```json
{
  "Elasticsearch": {
    "Url": "http://iccloudbackup.elasticsearch.production.pdm.local:9200",
    "DefaultIndex": "intelligencecenter",
    "RequestTimeout": "00:01:00"
  }
}
```

### Environment-Specific Settings

- `appsettings.Development.json` for development environment
- `appsettings.Production.json` for production environment (create as needed)

## Dependencies

### NuGet Packages

- `Microsoft.AspNetCore.OpenApi` (9.0.5) - OpenAPI/Swagger support
- `NEST` (5.6.1) - Elasticsearch .NET client

### Target Framework

- .NET 9.0

## Architecture Details

### Dependency Injection Configuration

Both services are configured with:

- Elasticsearch client as singleton
- Repository and service layers as scoped
- CORS enabled for cross-origin requests
- Comprehensive logging

### Error Handling

- Global exception handling
- Validation of query parameters
- Elasticsearch connection error handling
- Graceful degradation when external services are unavailable

### Performance Considerations

- Connection pooling for Elasticsearch
- Configurable request timeouts
- Pagination to handle large result sets
- Efficient field selection from Elasticsearch

## Troubleshooting

### Common Issues

1. **Connection Refused**: Check if Elasticsearch is accessible at the configured URL
2. **No Results**: Verify index names and field mappings
3. **Port Conflicts**: Ensure ports 5137 and 5215 are available
4. **Build Errors**: Verify .NET 9.0 SDK is installed

### Logs

Application logs include:

- Elasticsearch query details
- Response times
- Error details
- Request/response information

## Future Enhancements

### Planned Features

- Authentication and authorization
- Caching layer (Redis)
- Rate limiting
- Monitoring and metrics (Application Insights)
- Docker containerization
- Unit and integration tests
- OpenAPI documentation generation

### Performance Optimizations

- Result caching
- Elasticsearch query optimization
- Response compression
- Async/await pattern optimization

## Security Considerations

- Input validation and sanitization
- SQL injection prevention (though using NoSQL)
- CORS configuration review
- Authentication implementation
- HTTPS enforcement for production

---

**Last Updated**: May 31, 2025  
**Version**: 1.0.0  
**Maintainer**: Development Team
