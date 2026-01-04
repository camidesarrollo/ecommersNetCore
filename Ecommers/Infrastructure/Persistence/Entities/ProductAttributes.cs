using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductAttributes
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public long ValueId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Products Product { get; set; } = null!;

    public virtual AttributeValues Value { get; set; } = null!;
}
