import React, { useState, useEffect, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import { apiService } from '../services/api';
import { DealsListingResponse, DealsListingRequest } from '../types/api';
import DealCard from '../components/deals/DealCard';
import Pagination from '../components/common/Pagination';
import {
  PageContainer,
  PageTitle,
  FilterContainer,
  FilterGroup,
  FilterLabel,
  FilterInput,
  FilterSelect,
  FilterButton,
  GridContainer,
  LoadingSpinner,
  ErrorMessage,
  NoResultsMessage,
  Button,
  FlexContainer
} from '../components/styled/StyledComponents';

const DealsPage: React.FC = () => {
  const navigate = useNavigate();
  const [deals, setDeals] = useState<DealsListingResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<DealsListingRequest>({
    pageNumber: 1,
    pageSize: 20,
    query: '',
    country: '',
    dealType: '',
    status: '',
    minDealValue: undefined,
    maxDealValue: undefined,
  });

  const dealTypes = [
    'Merger & Acquisition',
    'Joint Venture',
    'Strategic Alliance',
    'IPO',
    'Private Equity',
    'Venture Capital',
    'Asset Acquisition',
    'Divestiture',
  ];

  const statuses = [
    'Announced',
    'Completed',
    'Pending',
    'Cancelled',
    'Rumoured',
  ];

  const countries = [
    'United States',
    'United Kingdom',
    'Germany',
    'France',
    'Japan',
    'China',
    'Canada',
    'Australia',
    'India',
    'Brazil',
  ];

  const fetchDeals = useCallback(async (searchFilters?: DealsListingRequest) => {
    try {
      setLoading(true);
      setError(null);
      
      const params = searchFilters || filters;
      const response = await apiService.getDeals(params);
      setDeals(response);
    } catch (err) {
      setError('Failed to fetch deals. Please try again.');
      console.error('Error fetching deals:', err);
    } finally {
      setLoading(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []); // Remove filters dependency to prevent auto-refresh

  useEffect(() => {
    // Only fetch deals on initial load with default filters
    const initialFilters: DealsListingRequest = {
      pageNumber: 1,
      pageSize: 20,
      query: '',
      country: '',
      dealType: '',
      status: '',
      minDealValue: undefined,
      maxDealValue: undefined,
    };
    fetchDeals(initialFilters);
  }, [fetchDeals]);

  const handleFilterChange = useCallback((key: keyof DealsListingRequest, value: any) => {
    setFilters(prev => ({
      ...prev,
      [key]: value,
      pageNumber: key !== 'pageNumber' ? 1 : value, // Reset to page 1 when changing filters
    }));
  }, []);

  const handleSearch = useCallback(() => {
    // Update filters to page 1 and then fetch
    const searchFilters = { ...filters, pageNumber: 1 };
    setFilters(searchFilters);
    fetchDeals(searchFilters);
  }, [filters, fetchDeals]);

  const handlePageChange = useCallback((page: number) => {
    const newFilters = { ...filters, pageNumber: page };
    setFilters(newFilters);
    fetchDeals(newFilters);
  }, [filters, fetchDeals]);

  const handleReset = useCallback(() => {
    const resetFilters: DealsListingRequest = {
      pageNumber: 1,
      pageSize: 20,
      query: '',
      country: '',
      dealType: '',
      status: '',
      minDealValue: undefined,
      maxDealValue: undefined,
    };
    setFilters(resetFilters);
    fetchDeals(resetFilters);
  }, [fetchDeals]);

  return (
    <PageContainer>
      <FlexContainer justify="space-between" align="center" style={{ marginBottom: '30px' }}>
        <PageTitle style={{ margin: 0 }}>Deals Intelligence</PageTitle>
        <Button onClick={() => navigate('/dashboard')} variant="secondary">
          📊 View Analytics Dashboard
        </Button>
      </FlexContainer>
      
      {/* Deals Filters and List */}
      <FilterContainer>
        <FilterGroup>
          <FilterLabel>Search Term</FilterLabel>
          <FilterInput
            type="text"
            placeholder="Search deals..."
            value={filters.query || ''}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleFilterChange('query', e.target.value)}
          />
        </FilterGroup>
        <FilterGroup>
          <FilterLabel>Country</FilterLabel>
          <FilterSelect
            value={filters.country || ''}
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) => handleFilterChange('country', e.target.value)}
          >
            <option value="">All Countries</option>
            {countries.map(country => (
              <option key={country} value={country}>{country}</option>
            ))}
          </FilterSelect>
        </FilterGroup>
        <FilterGroup>
          <FilterLabel>Deal Type</FilterLabel>
          <FilterSelect
            value={filters.dealType || ''}
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) => handleFilterChange('dealType', e.target.value)}
          >
            <option value="">All Types</option>
            {dealTypes.map(type => (
              <option key={type} value={type}>{type}</option>
            ))}
          </FilterSelect>
        </FilterGroup>
        <FilterGroup>
          <FilterLabel>Status</FilterLabel>
          <FilterSelect
            value={filters.status || ''}
            onChange={(e: React.ChangeEvent<HTMLSelectElement>) => handleFilterChange('status', e.target.value)}
          >
            <option value="">All Statuses</option>
            {statuses.map(status => (
              <option key={status} value={status}>{status}</option>
            ))}
          </FilterSelect>
        </FilterGroup>
        <FilterGroup>
          <FilterLabel>Min Value (USD)</FilterLabel>
          <FilterInput
            type="number"
            placeholder="e.g., 1000000"
            value={filters.minDealValue || ''}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleFilterChange('minDealValue', e.target.value ? parseFloat(e.target.value) : undefined)}
          />
        </FilterGroup>
        <FilterGroup>
          <FilterLabel>Max Value (USD)</FilterLabel>
          <FilterInput
            type="number"
            placeholder="e.g., 10000000"
            value={filters.maxDealValue || ''}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => handleFilterChange('maxDealValue', e.target.value ? parseFloat(e.target.value) : undefined)}
          />
        </FilterGroup>
        <FilterGroup>
          <FilterButton onClick={handleSearch}>Search</FilterButton>
          <FilterButton onClick={handleReset} style={{ marginLeft: '8px', backgroundColor: '#6c757d' }}>
            Reset
          </FilterButton>
        </FilterGroup>
      </FilterContainer>

      {loading && <LoadingSpinner>Loading deals...</LoadingSpinner>}
      {error && <ErrorMessage>{error}</ErrorMessage>}
      {!loading && !error && deals && (
        <>
          <div style={{ marginBottom: '20px', color: '#666' }}>
            Showing {deals.items.length} of {deals.totalItems} deals
          </div>
          {deals.items.length === 0 ? (
            <NoResultsMessage>
              No deals found matching your criteria. Try adjusting your filters.
            </NoResultsMessage>
          ) : (
            <>
              <GridContainer>
                {deals.items.map((deal) => (
                  <DealCard key={deal.baseDealId} deal={deal} />
                ))}
              </GridContainer>
              <Pagination
                currentPage={deals.pageNumber}
                totalPages={deals.totalPages}
                totalItems={deals.totalItems}
                pageSize={deals.pageSize}
                hasNextPage={deals.hasNextPage}
                hasPreviousPage={deals.hasPreviousPage}
                onPageChange={handlePageChange}
              />
            </>
          )}
        </>
      )}
    </PageContainer>
  );
};

export default DealsPage;
