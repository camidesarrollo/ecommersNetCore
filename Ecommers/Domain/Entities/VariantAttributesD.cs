using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class VariantAttributesD : BaseEntity<long>
    {
        public long VariantId { get; set; }

        public long AttributeId { get; set; }

        public long? ValueId { get; set; }

        public string? CustomValue { get; set; }
    }
}
