# Test script to display news listing results in a readable format
param(
    [int]$Page = 1,
    [int]$Size = 10
)

try {
    $response = Invoke-RestMethod -Uri "http://localhost:5137/api/news/listing?page=$Page&size=$Size" -Method Get -ContentType "application/json"
    
    Write-Host "================== NEWS LISTING RESPONSE ==================" -ForegroundColor Green
    Write-Host "Page: $($response.pageNumber) of $($response.totalPages)" -ForegroundColor Yellow
    Write-Host "Page Size: $($response.pageSize)" -ForegroundColor Yellow
    Write-Host "Total Items: $($response.totalItems)" -ForegroundColor Yellow
    Write-Host "Has Next Page: $($response.hasNextPage)" -ForegroundColor Yellow
    Write-Host "Has Previous Page: $($response.hasPreviousPage)" -ForegroundColor Yellow
    Write-Host ""
    
    Write-Host "================== NEWS ITEMS ==================" -ForegroundColor Green
    
    for ($i = 0; $i -lt $response.items.Count; $i++) {
        $item = $response.items[$i]
        Write-Host "[$($i + 1)] News Article ID: $($item.newsArticleId)" -ForegroundColor Cyan
        Write-Host "    Title: $($item.title)" -ForegroundColor White
        Write-Host "    Published Date: $($item.publishedDate)" -ForegroundColor Gray
        Write-Host "    URL Node: $($item.urlNode)" -ForegroundColor Gray
        Write-Host "    Sentiment: $($item.sentiment)" -ForegroundColor Magenta
        
        if ($item.newsEventTypes -and $item.newsEventTypes.Count -gt 0) {
            Write-Host "    Categories: $($item.newsEventTypes -join ', ')" -ForegroundColor Blue
        }
        
        if ($item.relatedCompanyNames -and $item.relatedCompanyNames.Count -gt 0) {
            Write-Host "    Companies: $($item.relatedCompanyNames -join ', ')" -ForegroundColor Green
        }
        
        if ($item.locations -and $item.locations.Count -gt 0) {
            Write-Host "    Locations: $($item.locations -join ', ')" -ForegroundColor DarkYellow
        }
        
        Write-Host ""
    }
    
    Write-Host "================== END OF RESULTS ==================" -ForegroundColor Green
}
catch {
    Write-Host "Error calling API: $_" -ForegroundColor Red
}
