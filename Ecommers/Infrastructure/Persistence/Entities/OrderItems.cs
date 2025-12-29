using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class OrderItems
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long ProductVariantsId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public string ProductSnapshot { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Orders Order { get; set; } = null!;

    public virtual ProductVariants ProductVariants { get; set; } = null!;
}
