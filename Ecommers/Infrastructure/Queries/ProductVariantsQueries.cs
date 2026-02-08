using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductVariantsQueries : CommonQueries<ProductVariants>
    {
        public static List<ProductVariants> GetProductVariantsByProduct(EcommersContext context, long ProductId)
        {

            var product = context.ProductVariants.Where(x => x.ProductId == ProductId).ToList();

            return product;
        }

        public static ProductVariants? GetProductVariantById(
         EcommersContext context,
         long productVariantId)
        {
            return context.ProductVariants
                .AsSplitQuery()

                .Include(v => v.ProductPriceHistory)

                // Imágenes del variant
                .Include(v => v.ProductVariantImages)

                // Atributos del variant
                .Include(v => v.VariantAttributes)
                    .ThenInclude(va => va.Attribute)

                .Include(v => v.VariantAttributes)
                    .ThenInclude(va => va.Value)

                .FirstOrDefault(v => v.Id == productVariantId);
        }

    }
}
