using Ecommers.Domain.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class ProductsDetailsViewModel
    {
        public Persistence.Entities.Products Products { get; set; }
        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];
    }
}
