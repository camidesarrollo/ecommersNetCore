using Ecommers.Domain.Entities;

namespace Ecommers.Web.Models.Products
{
    public class ProductsDetailsViewModel
    {
        public Ecommers.Infrastructure.Persistence.Entities.Products Products { get; set; }
        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];
    }
}
