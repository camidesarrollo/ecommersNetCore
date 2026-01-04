using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class Products
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public long CategoryId { get; set; }

    public decimal? BasePrice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual ICollection<ProductAttributes> ProductAttributes { get; set; } = new List<ProductAttributes>();

    public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();

    public virtual ICollection<ProductVariants> ProductVariants { get; set; } = new List<ProductVariants>();
}
