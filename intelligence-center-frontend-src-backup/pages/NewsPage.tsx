import React, { useState, useEffect, useCallback } from 'react';
import styled from 'styled-components';
import { apiService } from '../services/api';
import { NewsItem, NewsListingRequest } from '../types/api';
import {
  Container,
  Title,
  Button,
  Input,
  LoadingSpinner,
  ErrorMessage,
  Grid,
  FlexContainer,
  Card
} from '../components/styled/StyledComponents';
import NewsCard from '../components/news/NewsCard';
import Pagination from '../components/common/Pagination';

const FiltersContainer = styled(Card)`
  margin-bottom: 20px;
`;

const FilterGrid = styled(Grid)`
  grid-template-columns: 1fr 1fr auto;
  gap: 15px;
  align-items: end;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const LoadingContainer = styled.div`
  text-align: center;
  padding: 40px 0;
`;

const NewsPage: React.FC = () => {
  const [newsItems, setNewsItems] = useState<NewsItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  
  // Pagination state
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(0);
  const [totalItems, setTotalItems] = useState(0);
  const [pageSize] = useState(10);
  const [hasNextPage, setHasNextPage] = useState(false);
  const [hasPreviousPage, setHasPreviousPage] = useState(false);
  
  // Filter state
  const [searchTerm, setSearchTerm] = useState('');
  const [company, setCompany] = useState('');
  const [dateFrom, setDateFrom] = useState('');
  const [dateTo, setDateTo] = useState('');  const fetchNews = useCallback(async (
    page: number = 1,    searchParams?: {
      query?: string;
      company?: string;
      dateFrom?: string;
      dateTo?: string;
    }
  ) => {
    try {
      setLoading(true);
      setError(null);
      
      const params: NewsListingRequest = {
        pageNumber: page,
        pageSize: pageSize,
      };      // Use provided search params or current state
      const actualQuery = searchParams?.query ?? searchTerm;
      const actualCompany = searchParams?.company ?? company;
      const actualDateFrom = searchParams?.dateFrom ?? dateFrom;
      const actualDateTo = searchParams?.dateTo ?? dateTo;      if (actualQuery.trim()) params.query = actualQuery.trim();
      if (actualCompany.trim()) params.company = actualCompany.trim();
      if (actualDateFrom) params.dateFrom = actualDateFrom;
      if (actualDateTo) params.dateTo = actualDateTo;

      const response = await apiService.news.getNewsList(params);
        setNewsItems(response.items);
      setCurrentPage(page); // Update current page state
      setTotalPages(response.totalPages);
      setTotalItems(response.totalItems);
      setHasNextPage(response.hasNextPage);
      setHasPreviousPage(response.hasPreviousPage);
    } catch (error) {
      console.error('Error fetching news:', error);
      setError('Failed to load news. Please check if the News API is running.');    } finally {
      setLoading(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pageSize]); // Only depend on pageSize, not on filter states to prevent auto-refresh

  useEffect(() => {
    // Only fetch news on initial load
    fetchNews(1);
  }, [fetchNews]);
  const handleSearch = useCallback(() => {
    setCurrentPage(1); // Reset to first page when searching
    fetchNews(1, { query: searchTerm, company, dateFrom, dateTo });
  }, [fetchNews, searchTerm, company, dateFrom, dateTo]);

  const handleClearFilters = useCallback(() => {
    setSearchTerm('');
    setCompany('');
    setDateFrom('');
    setDateTo('');
    setCurrentPage(1);
    // Fetch news immediately after clearing filters with empty params
    fetchNews(1, { query: '', company: '', dateFrom: '', dateTo: '' });
  }, [fetchNews]);
  const handleRetry = useCallback(() => {
    fetchNews(currentPage);
  }, [fetchNews, currentPage]);
  const handlePageChange = useCallback((page: number) => {
    setCurrentPage(page);
    fetchNews(page);
  }, [fetchNews]);

  return (
    <Container>
      <Title>News Center</Title>
      
      {/* Filters */}
      <FiltersContainer>
        <FilterGrid>
          <div>
            <label htmlFor="searchTerm">Search</label>            <Input
              id="searchTerm"
              type="text"
              placeholder="Search in headlines, summary, content..."
              value={searchTerm}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSearchTerm(e.target.value)}
              onKeyDown={(e: React.KeyboardEvent<HTMLInputElement>) => e.key === 'Enter' && handleSearch()}
            />
          </div>
          
          <div>
            <label htmlFor="company">Company</label>            <Input
              id="company"
              type="text"
              placeholder="Filter by company..."
              value={company}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setCompany(e.target.value)}
              onKeyDown={(e: React.KeyboardEvent<HTMLInputElement>) => e.key === 'Enter' && handleSearch()}
            />
          </div>
          
          <FlexContainer gap="10px">
            <Button onClick={handleSearch}>Search</Button>
            <Button variant="outline" onClick={handleClearFilters}>Clear</Button>
          </FlexContainer>
        </FilterGrid>
        
        <FilterGrid style={{ marginTop: '15px' }}>
          <div>
            <label htmlFor="dateFrom">From Date</label>            <Input
              id="dateFrom"
              type="date"
              value={dateFrom}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setDateFrom(e.target.value)}
            />
          </div>
          
          <div>
            <label htmlFor="dateTo">To Date</label>
            <Input
              id="dateTo"
              type="date"
              value={dateTo}
              onChange={(e: React.ChangeEvent<HTMLInputElement>) => setDateTo(e.target.value)}
            />
          </div>
          
          <div></div>
        </FilterGrid>
      </FiltersContainer>

      {/* Results */}
      {loading ? (
        <LoadingContainer>
          <LoadingSpinner />
          <p>Loading news...</p>
        </LoadingContainer>
      ) : error ? (
        <ErrorMessage>
          {error}
          <br />          <Button 
            onClick={handleRetry} 
            style={{ marginTop: '10px' }}
            variant="outline"
          >
            Retry
          </Button>
        </ErrorMessage>
      ) : (
        <>
          {newsItems.length > 0 ? (
            <>
              <Grid columns={1} gap="15px">
                {newsItems.map((newsItem) => (
                  <NewsCard
                    key={newsItem.newsArticleId}
                    news={newsItem}
                  />
                ))}
              </Grid>
              
              {totalPages > 1 && (
                <Pagination
                  currentPage={currentPage}
                  totalPages={totalPages}
                  totalItems={totalItems}
                  pageSize={pageSize}
                  hasNextPage={hasNextPage}
                  hasPreviousPage={hasPreviousPage}
                  onPageChange={handlePageChange}
                />
              )}
            </>
          ) : (
            <p>No news articles found. Try adjusting your search criteria.</p>
          )}
        </>
      )}
    </Container>
  );
};

export default NewsPage;
