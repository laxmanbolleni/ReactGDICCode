namespace DealsMicroservices.DTOs
{
    /// <summary>
    /// Data Transfer Object for paginated Deals Listing response
    /// </summary>
    public class DealsListingResponseDto
    {
        public List<DealItemDto> Items { get; set; } = [];
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
