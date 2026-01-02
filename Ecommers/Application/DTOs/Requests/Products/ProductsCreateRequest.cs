using Ecommers.Application.DTOs.Requests.ProductVariants;

namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductsCreateRequest
    {
        public string Name { get; set; } = null!;

        public string Slug { get; set; } = null!;

        public string? Description { get; set; }

        public string? ShortDescription { get; set; }

        public bool IsActive { get; set; }

        public long CategoryId { get; set; }

        public ICollection<ProductVariantsCreateRequest> ProductVariants { get; set; } = [];
    }
}
