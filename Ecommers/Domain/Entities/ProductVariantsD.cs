using Ecommers.Domain.Common;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Domain.Entities
{
    public class ProductVariantsD : BaseEntity<long>
    {

        public string SKU { get; set; } = null!;

        public long StockQuantity { get; set; }

        public bool ManageStock { get; set; }

        public string StockStatus { get; set; } = null!;

        public bool IsFeatured { get; set; }

        public long ProductId { get; set; }

    }
}
