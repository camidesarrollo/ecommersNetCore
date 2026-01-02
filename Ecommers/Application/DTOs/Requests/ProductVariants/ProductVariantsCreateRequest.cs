namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsCreateRequest
    {
        public string SKU { get; set; } = null!;

        public long? StockQuantity { get; set; }

        public bool? ManageStock { get; set; }

        public string StockStatus { get; set; } = null!;

        public bool? IsFeatured { get; set; }

        public bool IsActive { get; set; }

        public long? ProductId { get; set; }
    }
}
