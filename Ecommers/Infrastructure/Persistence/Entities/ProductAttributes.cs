using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductAttributes
{
    public long Id { get; set; }

    public long VariantId { get; set; }

    public long ValueId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual AttributeValues Value { get; set; } = null!;

    public virtual Products Variant { get; set; } = null!;
}
