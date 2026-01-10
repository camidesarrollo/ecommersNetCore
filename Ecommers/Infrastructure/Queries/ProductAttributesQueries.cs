using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductAttributesQueries : CommonQueries<ProductAttributes>
    {
        public static List<ProductAttributes> GetProductAttributesByProduct(EcommersContext context, long ProductId)
        {

            var product = context.ProductAttributes.Where(x => x.ProductId == ProductId).ToList();

            return product;
        }
    }
}
