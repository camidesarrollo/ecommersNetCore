using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class AttributeValuesD : BaseEntity<long>
    {
        public long AttributeId { get; set; }

        public string? ValueString { get; set; }
        public string? ValueText { get; set; }
        public decimal? ValueDecimal { get; set; }
        public int? ValueInt { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateOnly? ValueDate { get; set; }

        public int DisplayOrder { get; set; }
    }
}
