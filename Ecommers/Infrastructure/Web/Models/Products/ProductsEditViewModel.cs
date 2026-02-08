using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class ProductsEditViewModel
    {
        public required Persistence.Entities.Products Products { get; set; }


        public IEnumerable<CategoriesD> Categories { get; set; } = [];

        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];

        public IEnumerable<AttributeValuesD> AtrributeValue { get; set; } = [];

        public IEnumerable<ProductImagesD> ProductImage { get; set; } = [];
        public IEnumerable<ProductVariantImagesD> ProductVariantImages { get; set; } = [];

        


    }
}
