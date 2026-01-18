namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsCreateRequest
    {
        public long ProductId { get; set; }

        public string SKU { get; set; } = null!;

        public string? Name { get; set; }

        public decimal Price { get; set; }

        public decimal? CompareAtPrice { get; set; }

        public decimal? CostPrice { get; set; }

        public long StockQuantity { get; set; }

        public bool ManageStock { get; set; }

        public string StockStatus { get; set; } = null!;

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }
    }
}
