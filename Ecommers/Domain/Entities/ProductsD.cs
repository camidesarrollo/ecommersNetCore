namespace Ecommers.Domain.Entities
{
    public class ProductsD
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string? Description { get; set; }

        public string? ShortDescription { get; set; }

        public bool IsActive { get; set; }

        public bool IsFeatured { get; set; }

        public long CategoryId { get; set; }
    }
}
