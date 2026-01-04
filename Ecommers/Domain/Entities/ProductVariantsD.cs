using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductVariantsD : BaseEntity<long>
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

    }
}
