namespace NewsMicroservices.DTOs
{
    /// <summary>
    /// Request parameters for News Listing endpoint
    /// </summary>
    public class NewsListingRequestDto
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;

        // Alias properties to match frontend parameter naming
        public int PageNumber
        {
            get => Page;
            set => Page = value;
        }

        public int PageSize
        {
            get => Size;
            set => Size = value;
        }

        public string? Query { get; set; }
        public List<string>? Categories { get; set; }
        public List<string>? Companies { get; set; }
        public List<string>? Locations { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
