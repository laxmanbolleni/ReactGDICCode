import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import styled from 'styled-components';
import { apiService } from '../services/api';
import { NewsItem, DealItem } from '../types/api';
import {
  Container,
  Title,
  Subtitle,
  Button,
  LoadingSpinner,
  ErrorMessage,
  Grid,
  FlexContainer
} from '../components/styled/StyledComponents';
import NewsCard from '../components/news/NewsCard';
import DealCard from '../components/deals/DealCard';

const Section = styled.section`
  margin: 40px 0;
`;

const SectionHeader = styled(FlexContainer)`
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
`;

const LoadingContainer = styled.div`
  text-align: center;
  padding: 40px 0;
`;

const HomePage: React.FC = () => {
  const navigate = useNavigate();
  const [newsItems, setNewsItems] = useState<NewsItem[]>([]);
  const [dealItems, setDealItems] = useState<DealItem[]>([]);
  const [newsLoading, setNewsLoading] = useState(true);
  const [dealsLoading, setDealsLoading] = useState(true);
  const [newsError, setNewsError] = useState<string | null>(null);
  const [dealsError, setDealsError] = useState<string | null>(null);

  useEffect(() => {
    fetchLatestNews();
    fetchLatestDeals();
  }, []);

  const fetchLatestNews = async () => {
    try {
      setNewsLoading(true);
      setNewsError(null);
      const response = await apiService.news.getNewsList({
        pageNumber: 1,
        pageSize: 5
      });
      setNewsItems(response.items);
    } catch (error) {
      console.error('Error fetching news:', error);
      setNewsError('Failed to load latest news. Please check if the News API is running.');
    } finally {
      setNewsLoading(false);
    }
  };

  const fetchLatestDeals = async () => {
    try {
      setDealsLoading(true);
      setDealsError(null);
      const response = await apiService.deals.getDealsList({
        pageNumber: 1,
        pageSize: 5
      });
      setDealItems(response.items);
    } catch (error) {
      console.error('Error fetching deals:', error);
      setDealsError('Failed to load latest deals. Please check if the Deals API is running.');
    } finally {
      setDealsLoading(false);
    }
  };
  return (
    <Container>
      <Title>GlobalData Intelligence Center</Title>
      
      {/* Side by Side Layout for News and Deals */}
      <Grid columns={2} gap="40px">
        {/* Latest News Section */}
        <Section>
          <SectionHeader>
            <Subtitle>Latest News</Subtitle>
            <Button onClick={() => navigate('/news')}>View All</Button>
          </SectionHeader>
          
          {newsLoading ? (
            <LoadingContainer>
              <LoadingSpinner />
              <p>Loading latest news...</p>
            </LoadingContainer>
          ) : newsError ? (
            <ErrorMessage>
              {newsError}
              <br />
              <Button 
                onClick={fetchLatestNews} 
                style={{ marginTop: '10px' }}
                variant="outline"
              >
                Retry
              </Button>
            </ErrorMessage>
          ) : newsItems.length > 0 ? (
            <Grid columns={1} gap="15px">
              {newsItems.map((newsItem) => (
                <NewsCard
                  key={newsItem.newsArticleId}
                  news={newsItem}
                  onClick={() => navigate('/news')}
                />
              ))}
            </Grid>
          ) : (
            <p>No news items available.</p>
          )}
        </Section>

        {/* Latest Deals Section */}
        <Section>
          <SectionHeader>
            <Subtitle>Latest Deals</Subtitle>
            <Button onClick={() => navigate('/deals')}>View All</Button>
          </SectionHeader>
          
          {dealsLoading ? (
            <LoadingContainer>
              <LoadingSpinner />
              <p>Loading latest deals...</p>
            </LoadingContainer>
          ) : dealsError ? (
            <ErrorMessage>
              {dealsError}
              <br />
              <Button 
                onClick={fetchLatestDeals} 
                style={{ marginTop: '10px' }}
                variant="outline"
              >
                Retry
              </Button>
            </ErrorMessage>
          ) : dealItems.length > 0 ? (
            <Grid columns={1} gap="15px">
              {dealItems.map((dealItem) => (
                <DealCard
                  key={dealItem.baseDealId}
                  deal={dealItem}
                  onClick={() => navigate('/deals')}
                />
              ))}
            </Grid>
          ) : (
            <p>No deals available.</p>
          )}
        </Section>
      </Grid>
    </Container>
  );
};

export default HomePage;
