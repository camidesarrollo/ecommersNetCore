using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductVariantImages
{
    public long Id { get; set; }

    public long VariantId { get; set; }

    public string Url { get; set; } = null!;

    public string? AltText { get; set; }

    public bool IsPrimary { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ProductVariants Variant { get; set; } = null!;
}
