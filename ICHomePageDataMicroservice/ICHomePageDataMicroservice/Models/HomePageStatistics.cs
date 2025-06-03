namespace ICHomePageDataMicroservice.Models
{
    public class HomePageStatistics
    {
        public long TotalNewsArticles { get; set; }
        public long TotalDeals { get; set; }
        public long TotalReports { get; set; }
        public long TotalClinicalTrials { get; set; }
        public long TotalCompanies { get; set; }
        public long TotalInvestigators { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class HomePageStatisticsResponse
    {
        public long TotalNewsArticles { get; set; }
        public long TotalDeals { get; set; }
        public long TotalReports { get; set; }
        public long TotalClinicalTrials { get; set; }
        public long TotalCompanies { get; set; }
        public long TotalInvestigators { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = "Statistics retrieved successfully";
    }

    public class FeaturedContentItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty; // "News", "Deal", "Report", etc.
        public string Url { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Priority { get; set; }
        public bool IsActive { get; set; }
    }

    public class FeaturedContentResponse
    {
        public List<FeaturedContentItem> Items { get; set; } = new List<FeaturedContentItem>();
        public int TotalCount { get; set; }
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = "Featured content retrieved successfully";
    }

    public class RecentActivityItem
    {
        public int Id { get; set; }
        public string ActivityType { get; set; } = string.Empty; // "News", "Deal", "Report"
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ActivityDate { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class RecentActivityResponse
    {
        public List<RecentActivityItem> Activities { get; set; } = new List<RecentActivityItem>();
        public int TotalCount { get; set; }
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = "Recent activities retrieved successfully";
    }

    public class HealthCheckResponse
    {
        public string Status { get; set; } = "Healthy";
        public string Service { get; set; } = "ICHomePageData";
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Version { get; set; } = "1.0.0";
        public Dictionary<string, object> Details { get; set; } = new Dictionary<string, object>();
    }

    // Models for stored procedure responses
    public class HomePageCarouselItem
    {
        public int CarouselId { get; set; }
        public string CarouselName { get; set; } = string.Empty;
        public string CarouselImageURL { get; set; } = string.Empty;
        public string CarouselLinkText { get; set; } = string.Empty;
        public string CarouselURL { get; set; } = string.Empty;
        public string CarouselDescription { get; set; } = string.Empty;
        public int CarouselSequence { get; set; }
    }

    public class HomePageCarouselResponse
    {
        public List<HomePageCarouselItem> Items { get; set; } = new List<HomePageCarouselItem>();
        public int TotalCount { get; set; }
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = "Carousel items retrieved successfully";
    }

    public class HomePageKeyStats
    {
        public string Name { get; set; } = string.Empty;
        public long Value { get; set; }
    }

    public class HomePageKeyStatsResponse
    {
        public List<HomePageKeyStats> Stats { get; set; } = new List<HomePageKeyStats>();
        public int TotalCount { get; set; }
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = "Key statistics retrieved successfully";
    }
}
