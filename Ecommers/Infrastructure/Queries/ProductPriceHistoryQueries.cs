using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductPriceHistoryQueries : CommonQueries<ProductPriceHistory>
    {
        public static List<ProductPriceHistory> GetProductPriceHistoryByVariante(EcommersContext context, long VariantId)
        {

            var product = context.ProductPriceHistory.Where(x => x.VariantId == VariantId).ToList();

            return product;
        }
    }
}
