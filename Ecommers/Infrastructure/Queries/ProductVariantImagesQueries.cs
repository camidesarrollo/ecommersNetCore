using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductVariantImagesQueries : CommonQueries<ProductVariants>
    {
        public static List<ProductVariantImages> GetProductVariantImagesByVariante(EcommersContext context, long VariantId)
        {

            var product = context.ProductVariantImages.Where(x => x.VariantId == VariantId).ToList();

            return product;
        }
    }
}
