import React, { memo, useMemo } from 'react';
import { Bar, Doughnut } from 'react-chartjs-2';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import styled from 'styled-components';
import { ChartData } from '../../types/dashboard';
import { Card, Subtitle } from '../styled/StyledComponents';

// Register Chart.js components
ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  ArcElement,
  Title,
  Tooltip,
  Legend
);

const ChartWrapper = styled(Card)`
  display: flex;
  flex-direction: column;
  height: 100%;
  min-height: 400px;
  will-change: transform;
  contain: layout;
`;

const ChartTitle = styled(Subtitle)`
  margin: 0 0 20px 0;
  text-align: center;
  font-size: 18px;
`;

const ChartContainer = styled.div`
  flex: 1;
  position: relative;
  min-height: 350px;
`;

const LoadingContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  flex: 1;
  min-height: 350px;
  color: #666;
`;

interface DealChartProps {
  data: ChartData | null;
  title: string;
  type: 'bar' | 'doughnut';
  loading?: boolean;
}

const DealChart: React.FC<DealChartProps> = memo(({ data, title, type, loading = false }) => {
  const chartOptions = useMemo(() => {
    const baseOptions = {
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          position: 'top' as const,
        },
        tooltip: {
          enabled: true,
        },
      },
      animation: {
        duration: 750,
        easing: 'easeInOutQuart' as const,
      },
    };

    if (type === 'bar') {
      return {
        ...baseOptions,
        scales: {
          y: {
            beginAtZero: true,
          },
        },
      };
    }

    // Doughnut chart options
    return {
      ...baseOptions,
      cutout: '60%',
    };
  }, [type]);

  if (loading) {
    return (
      <ChartWrapper>
        <ChartTitle>{title}</ChartTitle>
        <LoadingContainer>Loading chart...</LoadingContainer>
      </ChartWrapper>
    );
  }

  if (!data || !data.labels.length) {
    return (
      <ChartWrapper>
        <ChartTitle>{title}</ChartTitle>
        <LoadingContainer>No data available</LoadingContainer>
      </ChartWrapper>
    );
  }

  const ChartComponent = type === 'bar' ? Bar : Doughnut;

  return (
    <ChartWrapper>
      <ChartTitle>{title}</ChartTitle>
      <ChartContainer>
        <ChartComponent data={data} options={chartOptions} />
      </ChartContainer>
    </ChartWrapper>
  );
});

DealChart.displayName = 'DealChart';

export default DealChart;
