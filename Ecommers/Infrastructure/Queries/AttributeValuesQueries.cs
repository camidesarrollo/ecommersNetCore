using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class AttributeValuesQueries : CommonQueries<Banners>
    {
        public static List<AttributeValues> GetAttributeValuesSinRelacion(
        EcommersContext context
    )
        {
            var attributeValues = context.AttributeValues
                .Where(av =>
                    !context.ProductAttributes.Any(pa =>
                        pa.ValueId == av.Id &&
                        pa.DeletedAt == null
                    )
                    &&
                    !context.VariantAttributes.Any(va =>
                        va.ValueId == av.Id &&
                        va.DeletedAt == null
                    )
                )
                .Where(av => av.DeletedAt == null)
                .ToList();

            return attributeValues;
        }
    }
}
