using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class VariantAttributes
{
    public long Id { get; set; }

    public long VariantId { get; set; }

    public long AttributeId { get; set; }

    public long? ValueId { get; set; }

    public string? CustomValue { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual MasterAttributes Attribute { get; set; } = null!;

    public virtual AttributeValues? Value { get; set; }

    public virtual ProductVariants Variant { get; set; } = null!;
}
