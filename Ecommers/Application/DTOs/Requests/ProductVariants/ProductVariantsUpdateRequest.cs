using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.VariantAttributes;

namespace Ecommers.Application.DTOs.Requests.ProductVariants
{
    public class ProductVariantsUpdateRequest : ProductVariantsBaseRequest
    {

        public List<ProductVariantAttributeVM> Attributes { get; set; } = [];

        public List<ProductVariantImagesUpdateRequest> ProductVariantImages { get; set; } = [];
    }
}
