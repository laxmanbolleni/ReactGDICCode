# Test script for DealsMicroservices API
# This script tests all endpoints of the Deals microservice

Write-Host "=====================================" -ForegroundColor Green
Write-Host "Testing DealsMicroservices API" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

$baseUrl = "http://localhost:5215"

# Test 1: Health Check
Write-Host "`n1. Testing Health Check..." -ForegroundColor Yellow
try {
    $healthResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/health" -Method GET
    Write-Host "   Status: $($healthResponse.status)" -ForegroundColor Green
    Write-Host "   Timestamp: $($healthResponse.timestamp)" -ForegroundColor Green
} catch {
    Write-Host "   Health check failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Basic Deals Listing
Write-Host "`n2. Testing Basic Deals Listing..." -ForegroundColor Yellow
try {
    $listingResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing" -Method GET
    Write-Host "   Total Items: $($listingResponse.totalItems)" -ForegroundColor Green
    Write-Host "   Page Number: $($listingResponse.pageNumber)" -ForegroundColor Green
    Write-Host "   Page Size: $($listingResponse.pageSize)" -ForegroundColor Green
    Write-Host "   Total Pages: $($listingResponse.totalPages)" -ForegroundColor Green
    
    if ($listingResponse.items -and $listingResponse.items.Count -gt 0) {
        Write-Host "   Sample Deal:" -ForegroundColor Cyan
        $sampleDeal = $listingResponse.items[0]
        Write-Host "     - Base Deal ID: $($sampleDeal.baseDealId)" -ForegroundColor Cyan
        Write-Host "     - Title: $($sampleDeal.title)" -ForegroundColor Cyan
        Write-Host "     - Published Date: $($sampleDeal.publishedDate)" -ForegroundColor Cyan
        Write-Host "     - Deal Value: $($sampleDeal.dealValue)" -ForegroundColor Cyan
        Write-Host "     - Deal Type: $($sampleDeal.dealType)" -ForegroundColor Cyan
        Write-Host "     - Deal Status: $($sampleDeal.dealStatus)" -ForegroundColor Cyan
        Write-Host "     - Country: $($sampleDeal.dealCountryvalue)" -ForegroundColor Cyan
    } else {
        Write-Host "   No deals found in the response" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   Deals listing failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Pagination Test
Write-Host "`n3. Testing Pagination..." -ForegroundColor Yellow
try {
    $paginationResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?PageNumber=1&PageSize=5" -Method GET
    Write-Host "   Requested Page Size: 5" -ForegroundColor Green
    Write-Host "   Actual Page Size: $($paginationResponse.pageSize)" -ForegroundColor Green
    Write-Host "   Items Returned: $($paginationResponse.items.Count)" -ForegroundColor Green
} catch {
    Write-Host "   Pagination test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Search with Filters
Write-Host "`n4. Testing Search with Filters..." -ForegroundColor Yellow

# Test Country Filter
try {
    $countryResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?Country=USA" -Method GET
    Write-Host "   Country Filter (USA): $($countryResponse.totalItems) items" -ForegroundColor Green
} catch {
    Write-Host "   Country filter test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Deal Type Filter
try {
    $typeResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?DealType=Merger" -Method GET
    Write-Host "   Deal Type Filter (Merger): $($typeResponse.totalItems) items" -ForegroundColor Green
} catch {
    Write-Host "   Deal type filter test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test Status Filter
try {
    $statusResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?Status=Completed" -Method GET
    Write-Host "   Status Filter (Completed): $($statusResponse.totalItems) items" -ForegroundColor Green
} catch {
    Write-Host "   Status filter test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: Date Range Filter
Write-Host "`n5. Testing Date Range Filter..." -ForegroundColor Yellow
try {
    $dateFromStr = (Get-Date).AddDays(-30).ToString("yyyy-MM-dd")
    $dateToStr = (Get-Date).ToString("yyyy-MM-dd")
    $dateResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?DateFrom=$dateFromStr&DateTo=$dateToStr" -Method GET
    Write-Host "   Date Range ($dateFromStr to $dateToStr): $($dateResponse.totalItems) items" -ForegroundColor Green
} catch {
    Write-Host "   Date range filter test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 6: Deal Value Range Filter
Write-Host "`n6. Testing Deal Value Range Filter..." -ForegroundColor Yellow
try {
    $valueResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?MinDealValue=1000000&MaxDealValue=10000000" -Method GET
    Write-Host "   Deal Value Range (1M-10M): $($valueResponse.totalItems) items" -ForegroundColor Green
} catch {
    Write-Host "   Deal value range filter test failed: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 7: Combined Filters
Write-Host "`n7. Testing Combined Filters..." -ForegroundColor Yellow
try {
    $combinedResponse = Invoke-RestMethod -Uri "$baseUrl/api/deals/listing?Country=USA&DealType=Acquisition&PageSize=3" -Method GET
    Write-Host "   Combined Filters (USA + Acquisition, PageSize=3): $($combinedResponse.totalItems) items" -ForegroundColor Green
} catch {
    Write-Host "   Combined filters test failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=====================================" -ForegroundColor Green
Write-Host "API Testing Complete" -ForegroundColor Green
Write-Host "=====================================" -ForegroundColor Green

# Summary
Write-Host "`nAPI ENDPOINTS SUMMARY:" -ForegroundColor Magenta
Write-Host "Health Check: GET $baseUrl/api/deals/health" -ForegroundColor White
Write-Host "Deals Listing: GET $baseUrl/api/deals/listing" -ForegroundColor White
Write-Host "`nSUPPORTED QUERY PARAMETERS:" -ForegroundColor Magenta
Write-Host "- PageNumber: Page number (default: 1)" -ForegroundColor White
Write-Host "- PageSize: Items per page (default: 10, max: 100)" -ForegroundColor White
Write-Host "- Country: Filter by deal country" -ForegroundColor White
Write-Host "- DealType: Filter by deal type" -ForegroundColor White
Write-Host "- Status: Filter by deal status" -ForegroundColor White
Write-Host "- DateFrom: Filter deals from this date (yyyy-MM-dd)" -ForegroundColor White
Write-Host "- DateTo: Filter deals to this date (yyyy-MM-dd)" -ForegroundColor White
Write-Host "- MinDealValue: Minimum deal value" -ForegroundColor White
Write-Host "- MaxDealValue: Maximum deal value" -ForegroundColor White
