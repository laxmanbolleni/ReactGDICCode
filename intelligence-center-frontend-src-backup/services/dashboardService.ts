import { DashboardData, CountryAggregation, ChartData } from '../types/dashboard';

class DashboardService {
  private readonly dataUrl = '/data/deals-dashboard-data.json';

  /**
   * Fetch dashboard data from the mock JSON file
   */
  async getDashboardData(): Promise<DashboardData> {
    try {
      const response = await fetch(this.dataUrl);
      if (!response.ok) {
        throw new Error(`Failed to fetch dashboard data: ${response.statusText}`);
      }
      const data: DashboardData = await response.json();
      return data;
    } catch (error) {
      console.error('Error fetching dashboard data:', error);
      throw new Error('Unable to load dashboard data. Please try again later.');
    }
  }

  /**
   * Aggregate deals data by country for map visualization
   */
  aggregateByCountry(data: DashboardData): CountryAggregation[] {
    const countryMap = new Map<string, CountryAggregation>();

    data.deals.forEach(deal => {
      const existing = countryMap.get(deal.countryCode);
      if (existing) {
        existing.totalDeals += 1;
        existing.totalValue += deal.value;
        existing.deals.push(deal);
      } else {
        countryMap.set(deal.countryCode, {
          country: deal.country,
          countryCode: deal.countryCode,
          latitude: deal.latitude,
          longitude: deal.longitude,
          totalDeals: 1,
          totalValue: deal.value,
          deals: [deal]
        });
      }
    });

    return Array.from(countryMap.values());
  }

  /**
   * Prepare chart data for deal types
   */
  getDealTypeChartData(data: DashboardData): { volume: ChartData; value: ChartData } {
    const colors = [
      '#2e293d', // GlobalData primary
      '#0034ec', // GlobalData secondary
      '#28a745', // Success green
      '#ffc107', // Warning yellow
    ];

    const typeLabels = Object.keys(data.summary.dealsByType);
    const volumeData = Object.values(data.summary.dealsByType);
    const valueData = Object.values(data.summary.valueByType);

    return {
      volume: {
        labels: typeLabels,
        datasets: [{
          label: 'Number of Deals',
          data: volumeData,
          backgroundColor: colors,
          borderColor: colors.map(color => color + '80'),
          borderWidth: 1
        }]
      },
      value: {
        labels: typeLabels,
        datasets: [{
          label: 'Deal Value (USD)',
          data: valueData,
          backgroundColor: colors,
          borderColor: colors.map(color => color + '80'),
          borderWidth: 2
        }]
      }
    };
  }

  /**
   * Prepare chart data for deal status
   */
  getDealStatusChartData(data: DashboardData): { volume: ChartData; value: ChartData } {
    const colors = [
      '#28a745', // Completed - green
      '#ffc107', // Pending - yellow
      '#17a2b8', // In Progress - blue
      '#dc3545', // Failed - red
    ];

    const statusLabels = Object.keys(data.summary.dealsByStatus);
    const volumeData = Object.values(data.summary.dealsByStatus);
    const valueData = Object.values(data.summary.valueByStatus);

    return {
      volume: {
        labels: statusLabels,
        datasets: [{
          label: 'Number of Deals',
          data: volumeData,
          backgroundColor: colors,
          borderColor: colors.map(color => color + '80'),
          borderWidth: 1
        }]
      },
      value: {
        labels: statusLabels,
        datasets: [{
          label: 'Deal Value (USD)',
          data: valueData,
          backgroundColor: colors,
          borderColor: colors.map(color => color + '80'),
          borderWidth: 2
        }]
      }
    };
  }

  /**
   * Format currency values for display
   */
  formatCurrency(value: number, currency: string = 'USD'): string {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: currency,
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(value);
  }

  /**
   * Format large numbers with appropriate units (M, B, T)
   */
  formatLargeNumber(value: number): string {
    if (value >= 1e12) {
      return `$${(value / 1e12).toFixed(1)}T`;
    } else if (value >= 1e9) {
      return `$${(value / 1e9).toFixed(1)}B`;
    } else if (value >= 1e6) {
      return `$${(value / 1e6).toFixed(1)}M`;
    } else if (value >= 1e3) {
      return `$${(value / 1e3).toFixed(1)}K`;
    } else {
      return `$${value.toLocaleString()}`;
    }
  }
}

export const dashboardService = new DashboardService();
