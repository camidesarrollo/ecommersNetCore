using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductsD : BaseEntity<long>
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string? Description { get; set; }

        public string? ShortDescription { get; set; }


        public bool IsFeatured { get; set; }

        public long CategoryId { get; set; }
    }
}
