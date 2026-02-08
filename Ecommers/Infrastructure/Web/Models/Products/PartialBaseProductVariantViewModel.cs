using Ecommers.Domain.Entities;

namespace Ecommers.Infrastructure.Web.Models.Products
{
    public class PartialBaseProductVariantViewModel
    {
        public int Index { get; set; }
        public IEnumerable<MasterAttributesD> MasterAttributes { get; set; } = [];

        public IEnumerable<AttributeValuesD> AtrributeValue { get; set; } = [];
    }
}
