using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class AttributeValues
{
    public long Id { get; set; }

    public long AttributeId { get; set; }

    public string? ValueString { get; set; }

    public string? ValueText { get; set; }

    public decimal? ValueDecimal { get; set; }

    public int? ValueInt { get; set; }

    public bool? ValueBoolean { get; set; }

    public DateOnly? ValueDate { get; set; }

    public int DisplayOrder { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual MasterAttributes Attribute { get; set; } = null!;

    public virtual ICollection<ProductAttributes> ProductAttributes { get; set; } = new List<ProductAttributes>();

    public virtual ICollection<VariantAttributes> VariantAttributes { get; set; } = new List<VariantAttributes>();
}
