using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.ProductVariants;
namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductsCreateRequest : ProductsBaseRequest
    {
  
    }

    public class ProductCreateVM
    {
        public ProductVM Products { get; set; }

        public List<ProductAttributeVM> ProductsAttributes { get; set; } = [];

        public List<ProductImageVM> ProductImagesD { get; set; } = [];


        public int? PrimaryImageIndex { get; set; }

        public List<ProductVariantVM> ProductVariants { get; set; } = [];
    }

    public class ProductVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Slug { get; set; }
        public int CategoryId { get; set; }
        public decimal BasePrice { get; set; }

        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
    }
}
