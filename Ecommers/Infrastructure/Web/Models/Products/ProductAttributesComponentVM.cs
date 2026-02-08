using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Domain.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class ProductAttributesComponentVM : AttributesComponentComponentVM
    {
     
        public List<ProductAttributeVM> ProductsAttributes { get; set; } = new();
        
    }
}
