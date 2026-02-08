using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.VariantAttributes;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class ProductVariantAttributesComponentVM : AttributesComponentComponentVM
    {
        public int Index { get; set; }
        public List<ProductVariantAttributeVM> ProductVariant { get; set; } = [];

    }
}
