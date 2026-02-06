using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.VariantAttributes;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsCreateRequest : ProductVariantsBaseRequest
    {
    }

    public class ProductVariantVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public string? SKU { get; set; }

        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public decimal? CompareAtPrice { get; set; }

        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }

        public List<ProductVariantAttributeVM> Attributes { get; set; } = [];

        public List<ProductVariantImageVM> ProductVariantImages { get; set; } = [];
    }
}
