using Ecommers.Domain.Entities;

namespace Ecommers.Web.Models.Products
{
    public class ProductsCreateViewModel
    {
        public Ecommers.Infrastructure.Persistence.Entities.Products Products { get; set; } = null!;

        public Ecommers.Infrastructure.Persistence.Entities.ProductVariants ProductVariants { get; set; } = null!;

        public IEnumerable<CategoriesD> Categories { get; set; } = [];

        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];


    }
}
