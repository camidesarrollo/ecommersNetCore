using Ecommers.Domain.Entities;

namespace Ecommers.Web.Models.Products
{
    public class ProductsCreateViewModel
    {
        public ProductsD Products { get; set; } = null!;

        public IEnumerable<ProductImagesD> ProductImages { get; set; } = [];

        public IEnumerable<CategoriesD> Categories { get; set; } = [];

        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];


    }
}
