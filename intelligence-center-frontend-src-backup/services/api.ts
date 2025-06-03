import axios from 'axios';
import {
  NewsListingResponse,
  DealsListingResponse,
  NewsListingRequest,
  DealsListingRequest,
  HealthResponse
} from '../types/api';

// API base URLs
const NEWS_API_BASE_URL = 'http://localhost:5137/api/news';
const DEALS_API_BASE_URL = 'http://localhost:5215/api/deals';

// Create axios instances
const newsApi = axios.create({
  baseURL: NEWS_API_BASE_URL,
  timeout: 10000,
});

const dealsApi = axios.create({
  baseURL: DEALS_API_BASE_URL,
  timeout: 10000,
});

// News API functions
export const newsService = {
  async getHealthCheck(): Promise<HealthResponse> {
    const response = await newsApi.get<HealthResponse>('/health');
    return response.data;
  },

  async getNewsList(params: NewsListingRequest = {}): Promise<NewsListingResponse> {
    const response = await newsApi.get<NewsListingResponse>('/listing', { params });
    return response.data;
  },
};

// Deals API functions
export const dealsService = {
  async getHealthCheck(): Promise<HealthResponse> {
    const response = await dealsApi.get<HealthResponse>('/health');
    return response.data;
  },

  async getDealsList(params: DealsListingRequest = {}): Promise<DealsListingResponse> {
    const response = await dealsApi.get<DealsListingResponse>('/listing', { params });
    return response.data;
  },

  async getDealsByCountry(): Promise<any> {
    const response = await dealsApi.get('/by-country');
    return response.data;
  },

  async getDealsByType(): Promise<any> {
    const response = await dealsApi.get('/by-type');
    return response.data;
  },

  async getDealsByStatus(): Promise<any> {
    const response = await dealsApi.get('/by-status');
    return response.data;
  },
};

// Combined service
export const apiService = {
  news: newsService,
  deals: dealsService,
  // Direct methods for backward compatibility
  getDeals: dealsService.getDealsList,
};
