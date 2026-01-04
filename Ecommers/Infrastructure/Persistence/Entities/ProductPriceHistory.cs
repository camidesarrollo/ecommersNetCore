using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductPriceHistory
{
    public long Id { get; set; }

    public long VariantId { get; set; }

    public decimal Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ProductVariants Variant { get; set; } = null!;
}
