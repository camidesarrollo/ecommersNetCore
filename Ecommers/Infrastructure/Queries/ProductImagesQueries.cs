using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductImagesQueries
    {
        public static List<ProductImages> GetProductImagesByProduct(EcommersContext context, long ProductId)
        {

            var product = context.ProductImages.Where(x => x.ProductId == ProductId).ToList();

            return product;
        }
    }
}
