import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { DashboardData, CountryAggregation, ChartData } from '../types/dashboard';
import { dashboardService } from '../services/dashboardService';
import {
  Container,
  Title,
  Grid,
  Card,
  Subtitle,
  Text,
  LoadingSpinner,
  ErrorMessage,
  Button
} from '../components/styled/StyledComponents';
import DealsWorldMap from '../components/dashboard/DealsWorldMap';
import DealChart from '../components/dashboard/DealsChart';

const DashboardContainer = styled(Container)`
  padding: 20px;
  max-width: 1400px;
`;

const StatsGrid = styled(Grid)`
  margin-bottom: 30px;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 20px;
`;

const StatCard = styled(Card)`
  text-align: center;
  padding: 24px;
  background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
  border-left: 4px solid #2e293d;
`;

const StatValue = styled.div`
  font-size: 2.5rem;
  font-weight: 700;
  color: #2e293d;
  margin: 10px 0;
  font-family: 'IBM Plex Sans', sans-serif;
`;

const StatLabel = styled(Text)`
  font-size: 14px;
  color: #666;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin: 0;
`;

const ChartsGrid = styled(Grid)`
  grid-template-columns: 1fr 1fr;
  gap: 20px;
  margin: 30px 0;

  @media (max-width: 768px) {
    grid-template-columns: 1fr;
  }
`;

const MapSection = styled.div`
  margin: 30px 0;
`;

const ErrorContainer = styled.div`
  text-align: center;
  padding: 40px;
`;

const LoadingContainer = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 60px 20px;
  text-align: center;
`;

const DashboardPage: React.FC = () => {
  const [dashboardData, setDashboardData] = useState<DashboardData | null>(null);
  const [countries, setCountries] = useState<CountryAggregation[]>([]);
  const [typeCharts, setTypeCharts] = useState<{ volume: ChartData; value: ChartData } | null>(null);
  const [statusCharts, setStatusCharts] = useState<{ volume: ChartData; value: ChartData } | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchDashboardData = async () => {
    try {
      setLoading(true);
      setError(null);
      
      // Fetch dashboard data
      const data = await dashboardService.getDashboardData();
      setDashboardData(data);

      // Process data for components
      const countryAggregations = dashboardService.aggregateByCountry(data);
      setCountries(countryAggregations);

      const dealTypeCharts = dashboardService.getDealTypeChartData(data);
      setTypeCharts(dealTypeCharts);

      const dealStatusCharts = dashboardService.getDealStatusChartData(data);
      setStatusCharts(dealStatusCharts);

    } catch (err) {
      console.error('Error fetching dashboard data:', err);
      setError(err instanceof Error ? err.message : 'Failed to load dashboard data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchDashboardData();
  }, []);

  const formatCurrency = (value: number): string => {
    return dashboardService.formatLargeNumber(value);
  };

  const handleRetry = () => {
    fetchDashboardData();
  };

  if (loading) {
    return (
      <DashboardContainer>
        <LoadingContainer>
          <LoadingSpinner />
          <Title>Loading Dashboard...</Title>
          <Text>Fetching deals data and analytics...</Text>
        </LoadingContainer>
      </DashboardContainer>
    );
  }

  if (error) {
    return (
      <DashboardContainer>
        <ErrorContainer>
          <ErrorMessage>
            {error}
            <br />
            <Button 
              onClick={handleRetry} 
              style={{ marginTop: '20px' }}
              variant="outline"
            >
              Retry Loading
            </Button>
          </ErrorMessage>
        </ErrorContainer>
      </DashboardContainer>
    );
  }

  if (!dashboardData) {
    return (
      <DashboardContainer>
        <ErrorContainer>
          <Text>No dashboard data available</Text>
        </ErrorContainer>
      </DashboardContainer>
    );
  }

  return (
    <DashboardContainer>
      <Title>Deals Analytics Dashboard</Title>
      
      {/* Summary Statistics */}
      <StatsGrid>
        <StatCard>
          <StatLabel>Total Deals</StatLabel>
          <StatValue>{dashboardData.summary.totalDeals.toLocaleString()}</StatValue>
        </StatCard>
        
        <StatCard>
          <StatLabel>Total Value</StatLabel>
          <StatValue>${formatCurrency(dashboardData.summary.totalValue)}</StatValue>
        </StatCard>
        
        <StatCard>
          <StatLabel>Average Deal Size</StatLabel>
          <StatValue>
            ${formatCurrency(dashboardData.summary.totalValue / dashboardData.summary.totalDeals)}
          </StatValue>
        </StatCard>
        
        <StatCard>
          <StatLabel>Countries</StatLabel>
          <StatValue>{countries.length}</StatValue>
        </StatCard>
      </StatsGrid>      {/* World Map */}
      <Subtitle style={{ marginBottom: '20px', fontSize: '24px' }}>Global Deals Distribution</Subtitle>
      <MapSection>
        <DealsWorldMap countries={countries} loading={loading} />
      </MapSection>

      {/* Map Information Card */}
      <Card style={{ marginTop: '20px', padding: '20px', backgroundColor: '#f8f9fa' }}>
        <Subtitle style={{ fontSize: '18px', marginBottom: '15px' }}>World Map Aggregation Details</Subtitle>
        <Grid columns={3} gap="20px">
          <div>
            <Text><strong>Aggregation Method:</strong></Text>
            <Text>Data aggregated by country using country codes</Text>
            <Text><strong>Deals Volume:</strong> Total count of deals per country</Text>
            <Text><strong>Value:</strong> Sum of all deal values per country</Text>
          </div>
          <div>
            <Text><strong>Marker Sizing:</strong></Text>
            <Text>• Small markers: 1-2 deals</Text>
            <Text>• Medium markers: 3-4 deals</Text>
            <Text>• Large markers: 5+ deals</Text>
          </div>
          <div>
            <Text><strong>Interactive Features:</strong></Text>
            <Text>• Click markers for detailed country data</Text>
            <Text>• View deals volume and total value</Text>
            <Text>• Average deal size calculation</Text>
          </div>
        </Grid>
      </Card>      {/* Deal Type Charts */}
      <Subtitle style={{ marginBottom: '20px', fontSize: '24px' }}>Deal Type Analysis</Subtitle>
      <ChartsGrid>
        <DealChart
          data={typeCharts?.volume || null}
          title="Deal Volume by Type"
          type="bar"
          loading={loading}
        />
        <DealChart
          data={typeCharts?.value || null}
          title="Deal Value by Type"
          type="doughnut"
          loading={loading}
        />
      </ChartsGrid>

      {/* Deal Status Charts */}
      <Subtitle style={{ marginBottom: '20px', fontSize: '24px' }}>Deal Status Analysis</Subtitle>
      <ChartsGrid>
        <DealChart
          data={statusCharts?.volume || null}
          title="Deal Volume by Status"
          type="bar"
          loading={loading}
        />
        <DealChart
          data={statusCharts?.value || null}
          title="Deal Value by Status"
          type="doughnut"
          loading={loading}
        />
      </ChartsGrid>

      {/* Data Summary */}
      <Card style={{ marginTop: '30px', padding: '20px' }}>
        <Subtitle>Data Summary</Subtitle>
        <Grid columns={2} gap="20px" style={{ marginTop: '15px' }}>
          <div>
            <Text><strong>Last Updated:</strong> {new Date().toLocaleDateString()}</Text>
            <Text><strong>Data Source:</strong> GlobalData Intelligence Center</Text>
            <Text><strong>Coverage Period:</strong> 2024 Deals</Text>
          </div>
          <div>
            <Text><strong>Total Records:</strong> {dashboardData.deals.length} deals</Text>
            <Text><strong>Geographic Coverage:</strong> {countries.length} countries</Text>
            <Text><strong>Deal Types:</strong> {Object.keys(dashboardData.summary.dealsByType).length}</Text>
          </div>
        </Grid>
      </Card>
    </DashboardContainer>
  );
};

export default DashboardPage;
