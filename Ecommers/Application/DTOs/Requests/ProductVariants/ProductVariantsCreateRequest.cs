using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.VariantAttributes;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsCreateRequest : ProductVariantsBaseRequest
    {

        public List<ProductVariantAttributeVM> Attributes { get; set; } = [];

        public List<ProductVariantImagesCreateRequest> ProductVariantImages { get; set; } = [];
    }
}
