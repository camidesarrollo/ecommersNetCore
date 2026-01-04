using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductsD : BaseEntity<long>
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string ShortDescription { get; set; } = null!;

        public long CategoryId { get; set; }

        public decimal? BasePrice { get; set; }
    }
}
