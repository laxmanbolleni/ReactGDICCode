// Types for News API
export interface NewsItem {
  newsArticleId: number;
  title: string;
  summary?: string;
  content?: string;
  publishedDate: string;
  urlNode: string;
  companies?: NewsCompany[];
  relatedCompanyNames?: string[];
  newsEventTypes?: string[];
  locations?: string[];
  sentiment?: string;
}

export interface NewsCompany {
  companyId: number;
  companyName: string;
}

export interface NewsListingResponse {
  items: NewsItem[];
  pageNumber: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

// Types for Deals API
export interface DealItem {
  baseDealId: number;
  title: string;
  publishedDate: string;
  urlNode: string;
  dealCountryvalue: string;
  dealType: string;
  dealStatus: string;
  dealValue: number;
}

export interface DealsListingResponse {
  items: DealItem[];
  pageNumber: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

// Common API request parameters
export interface ListingRequest {
  pageNumber?: number;
  pageSize?: number;
  dateFrom?: string;
  dateTo?: string;
}

export interface NewsListingRequest extends ListingRequest {
  query?: string;
  company?: string;
}

export interface DealsListingRequest extends ListingRequest {
  country?: string;
  dealType?: string;
  status?: string;
  minDealValue?: number;
  maxDealValue?: number;
  query?: string;
}

// Health check response
export interface HealthResponse {
  status: string;
  timestamp: string;
}
