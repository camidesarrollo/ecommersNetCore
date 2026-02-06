using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Web.Models.Products
{
    public class ProductsCreateViewModel
    {
        public required Ecommers.Infrastructure.Persistence.Entities.Products Products { get; set; }

        public IEnumerable<ProductImagesD> ProductImage { get; set; } = [];

        public IEnumerable<CategoriesD> Categories { get; set; } = [];

        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];

        public IEnumerable<AttributeValuesD> AtrributeValue { get; set; } = [];

        public IEnumerable<ProductVariantImagesD> ProductVariantImages { get; set; } = [];


    }
}
