using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductVariantAttributes
{
    public long Id { get; set; }

    public long VariantId { get; set; }

    public long ValueId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual AttributeValues Value { get; set; } = null!;

    public virtual ProductVariants Variant { get; set; } = null!;
}
