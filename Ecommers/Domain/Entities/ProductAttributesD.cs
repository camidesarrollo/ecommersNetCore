using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class ProductAttributesD : BaseEntity<long>
    {
        public long ProductId { get; set; }

        public long AttributeId { get; set; }

        public long? ValueId { get; set; }

        public string? CustomValue { get; set; }

    }
}
