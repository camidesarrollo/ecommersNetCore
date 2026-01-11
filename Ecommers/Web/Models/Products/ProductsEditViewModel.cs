using Ecommers.Domain.Entities;

namespace Ecommers.Web.Models.Products
{
    public class ProductsEditViewModel
    {
        public required Ecommers.Infrastructure.Persistence.Entities.Products Products { get; set; }

        public IEnumerable<CategoriesD> Categories { get; set; } = [];

        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];

        public IEnumerable<AttributeValuesD> AtrributeValue { get; set; } = [];
    }
}
