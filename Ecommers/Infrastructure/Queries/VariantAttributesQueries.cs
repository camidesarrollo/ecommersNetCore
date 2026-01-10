using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class VariantAttributesQueries : CommonQueries<VariantAttributes>
    {
        public static List<VariantAttributes> GetVariantAttributesByVariante(EcommersContext context, long VariantId)
        {

            var product = context.VariantAttributes.Where(x => x.VariantId == VariantId).ToList();

            return product;
        }
    }
}
