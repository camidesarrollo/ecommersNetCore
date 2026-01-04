using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductsD : BaseEntity<long>
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public string SKU { get; set; } = null!;

        public long StockQuantity { get; set; }

        public bool ManageStock { get; set; }

        public string StockStatus { get; set; } = null!;

        public long CategoryId { get; set; }
    }
}
