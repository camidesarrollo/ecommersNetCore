using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.ProductVariants;
namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductsUpdateRequest : ProductsBaseRequest
    {
        [Required(ErrorMessage = "El identificador es obligatorio")]
        public long Id { get; set; }
    }

    public class ProductEditVM
    {
        public ProductsUpdateRequest Products { get; set; }

        public List<ProductAttributeVM> ProductsAttributes { get; set; } = [];

        public List<ProductImagesUpdateRequest> ProductImagesD { get; set; } = [];

        public int? PrimaryImageIndex { get; set; }

        public List<ProductVariantsUpdateRequest> ProductVariants { get; set; } = [];
    }
}
