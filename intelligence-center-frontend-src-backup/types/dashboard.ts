// Dashboard data types for deals analytics
export interface DashboardDeal {
  dealId: string;
  country: string;
  countryCode: string;
  latitude: number;
  longitude: number;
  dealType: 'Merger & Acquisition' | 'IPO' | 'Private Equity' | 'Venture Capital';
  dealStatus: 'Completed' | 'Pending' | 'In Progress' | 'Failed';
  value: number;
  currency: string;
  dealDate: string;
  companyName: string;
  sector: string;
}

export interface DashboardSummary {
  totalDeals: number;
  totalValue: number;
  dealsByStatus: Record<string, number>;
  dealsByType: Record<string, number>;
  valueByStatus: Record<string, number>;
  valueByType: Record<string, number>;
}

export interface DashboardData {
  deals: DashboardDeal[];
  summary: DashboardSummary;
}

export interface CountryAggregation {
  country: string;
  countryCode: string;
  latitude: number;
  longitude: number;
  totalDeals: number;
  totalValue: number;
  deals: DashboardDeal[];
}

export interface ChartDataPoint {
  label: string;
  value: number;
  color?: string;
}

// Chart.js compatible data structures
export interface ChartData {
  labels: string[];
  datasets: {
    label: string;
    data: number[];
    backgroundColor: string[];
    borderColor?: string[];
    borderWidth?: number;
  }[];
}
