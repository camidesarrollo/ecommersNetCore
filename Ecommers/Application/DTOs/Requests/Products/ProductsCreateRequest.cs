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
        public ProductsCreateRequest Products { get; set; }

        public List<ProductAttributeVM> ProductsAttributes { get; set; } = [];

        public List<ProductImagesCreateRequest> ProductImagesD { get; set; } = [];

        public int? PrimaryImageIndex { get; set; }

        public List<ProductVariantsCreateRequest> ProductVariants { get; set; } = [];
    }
}
