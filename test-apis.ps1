#!/usr/bin/env pwsh

Write-Host "=== GlobalData Intelligence Center API Testing ===" -ForegroundColor Green
Write-Host ""

# Test News API Health
Write-Host "Testing News API Health..." -ForegroundColor Yellow
try {
    $newsHealth = Invoke-RestMethod -Uri "http://localhost:5137/api/news/health" -Method Get
    Write-Host "✅ News API Health: $($newsHealth.status)" -ForegroundColor Green
    Write-Host "   Timestamp: $($newsHealth.timestamp)" -ForegroundColor Gray
} catch {
    Write-Host "❌ News API Health: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test News API Listing
Write-Host "Testing News API Listing..." -ForegroundColor Yellow
try {
    $newsListing = Invoke-RestMethod -Uri "http://localhost:5137/api/news/listing?page=1&size=3" -Method Get
    Write-Host "✅ News API Listing: Success" -ForegroundColor Green
    Write-Host "   Total Items: $($newsListing.totalItems)" -ForegroundColor Gray
    Write-Host "   Page: $($newsListing.pageNumber) of $($newsListing.totalPages)" -ForegroundColor Gray
    Write-Host "   Items returned: $($newsListing.items.Count)" -ForegroundColor Gray
    if ($newsListing.items.Count -gt 0) {
        Write-Host "   First item: $($newsListing.items[0].title)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ News API Listing: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test Deals API Health
Write-Host "Testing Deals API Health..." -ForegroundColor Yellow
try {
    $dealsHealth = Invoke-RestMethod -Uri "http://localhost:5215/api/deals/health" -Method Get
    Write-Host "✅ Deals API Health: $($dealsHealth.status)" -ForegroundColor Green
    Write-Host "   Timestamp: $($dealsHealth.timestamp)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Deals API Health: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test Deals API Listing
Write-Host "Testing Deals API Listing..." -ForegroundColor Yellow
try {
    $dealsListing = Invoke-RestMethod -Uri "http://localhost:5215/api/deals/listing?page=1&size=3" -Method Get
    Write-Host "✅ Deals API Listing: Success" -ForegroundColor Green
    Write-Host "   Total Items: $($dealsListing.totalItems)" -ForegroundColor Gray
    Write-Host "   Page: $($dealsListing.pageNumber) of $($dealsListing.totalPages)" -ForegroundColor Gray
    Write-Host "   Items returned: $($dealsListing.items.Count)" -ForegroundColor Gray
    if ($dealsListing.items.Count -gt 0) {
        Write-Host "   First item: $($dealsListing.items[0].title)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Deals API Listing: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# Test Deals API Aggregations
Write-Host "Testing Deals API Aggregations..." -ForegroundColor Yellow
try {
    $dealsByCountry = Invoke-RestMethod -Uri "http://localhost:5215/api/deals/by-country" -Method Get
    Write-Host "✅ Deals by Country: Success" -ForegroundColor Green
    Write-Host "   Countries returned: $($dealsByCountry.countries.Count)" -ForegroundColor Gray
    if ($dealsByCountry.countries.Count -gt 0) {
        $topCountry = $dealsByCountry.countries | Sort-Object dealVolume -Descending | Select-Object -First 1
        Write-Host "   Top country: $($topCountry.country) ($($topCountry.dealVolume) deals)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Deals by Country: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

try {
    $dealsByType = Invoke-RestMethod -Uri "http://localhost:5215/api/deals/by-type" -Method Get
    Write-Host "✅ Deals by Type: Success" -ForegroundColor Green
    Write-Host "   Types returned: $($dealsByType.types.Count)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Deals by Type: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

try {
    $dealsByStatus = Invoke-RestMethod -Uri "http://localhost:5215/api/deals/by-status" -Method Get
    Write-Host "✅ Deals by Status: Success" -ForegroundColor Green
    Write-Host "   Statuses returned: $($dealsByStatus.statuses.Count)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Deals by Status: Failed - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== API Testing Complete ===" -ForegroundColor Green
