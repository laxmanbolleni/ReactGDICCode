using System.ComponentModel.DataAnnotations;

namespace DealsMicroservices.DTOs
{
    /// <summary>
    /// Request parameters for Deals Listing endpoint
    /// </summary>
    public class DealsListingRequestDto
    {
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "PageSize must be between 1 and 100")]
        public int Size { get; set; } = 10;

        // Alias for PageSize to match API parameter naming
        public int PageSize
        {
            get => Size;
            set => Size = value;
        }

        public int PageNumber
        {
            get => Page;
            set => Page = value;
        }

        public string? Query { get; set; }
        public string? Country { get; set; }
        public string? DealType { get; set; }
        public string? Status { get; set; }
        public decimal? MinDealValue { get; set; }
        public decimal? MaxDealValue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
