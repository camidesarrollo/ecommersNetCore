using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductVariantsQueries : CommonQueries<ProductVariants>
    {
        public static List<ProductVariants> GetProductVariantsByProduct(EcommersContext context, long ProductId)
        {

            var product = context.ProductVariants.Where(x => x.ProductId == ProductId).ToList();

            return product;
        }
    }
}
