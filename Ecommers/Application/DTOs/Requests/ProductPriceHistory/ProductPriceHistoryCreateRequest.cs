namespace Ecommers.Application.DTOs.Requests.ProductPriceHistory
{
    public class ProductPriceHistoryCreateRequest
    {
        public long VariantId { get; set; }

        public decimal Price { get; set; }

        public decimal? CompareAtPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Reason { get; set; }

        public bool IsActive { get; set; }
    }
}
