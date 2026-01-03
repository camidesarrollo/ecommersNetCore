using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductVariants
{
    public long Id { get; set; }

    public string SKU { get; set; } = null!;

    public long StockQuantity { get; set; }

    public bool ManageStock { get; set; }

    public string StockStatus { get; set; } = null!;

    public bool IsFeatured { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long ProductId { get; set; }

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual Products Product { get; set; } = null!;

    public virtual ICollection<ProductVariantAttributes> ProductVariantAttributes { get; set; } = new List<ProductVariantAttributes>();

    public virtual ICollection<ProductVariantImages> ProductVariantImages { get; set; } = new List<ProductVariantImages>();

    public virtual ICollection<ProductVariantPriceHistory> ProductVariantPriceHistory { get; set; } = new List<ProductVariantPriceHistory>();
}
