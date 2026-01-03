using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class Products
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string? Description { get; set; }

    public string? ShortDescription { get; set; }

    public bool IsActive { get; set; }

    public long CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual ICollection<ProductVariants> ProductVariants { get; set; } = new List<ProductVariants>();
}
