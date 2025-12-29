using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class MasterAttributesD : BaseEntity<long>
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string Slug { get; set; } = null!;

        public string DataType { get; set; } = null!;

        public string InputType { get; set; } = null!;

        public string? Unit { get; set; }

        public bool IsRequired { get; set; }

        public bool IsFilterable { get; set; }

        public bool IsVariant { get; set; }

        public string? Category { get; set; }

        public int? CantidadProductos { get; set; }

        public bool? CanDelete { get; set; }
    }
}
