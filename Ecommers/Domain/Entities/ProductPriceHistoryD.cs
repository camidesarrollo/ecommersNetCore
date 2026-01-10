using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductPriceHistoryD : BaseEntity<long>
    {
        public long VariantId { get; set; }

        public decimal Price { get; set; }

        public decimal? CompareAtPrice { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? Reason { get; set; }
    }
}
