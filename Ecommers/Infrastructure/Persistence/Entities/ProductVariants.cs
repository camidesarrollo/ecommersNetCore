using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductVariants
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string SKU { get; set; } = null!;

    public string? Name { get; set; }

    public decimal Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public decimal? CostPrice { get; set; }

    public long StockQuantity { get; set; }

    public bool ManageStock { get; set; }

    public string StockStatus { get; set; } = null!;

    public bool IsDefault { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual Products Product { get; set; } = null!;

    public virtual ICollection<ProductPriceHistory> ProductPriceHistory { get; set; } = new List<ProductPriceHistory>();

    public virtual ICollection<ProductVariantImages> ProductVariantImages { get; set; } = new List<ProductVariantImages>();

    public virtual ICollection<VariantAttributes> VariantAttributes { get; set; } = new List<VariantAttributes>();
}
