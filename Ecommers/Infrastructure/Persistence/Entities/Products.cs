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

    public string SKU { get; set; } = null!;

    public long StockQuantity { get; set; }

    public bool ManageStock { get; set; }

    public string StockStatus { get; set; } = null!;

    public bool IsActive { get; set; }

    public long CategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Categories Category { get; set; } = null!;

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual ICollection<ProductAttributes> ProductAttributes { get; set; } = new List<ProductAttributes>();

    public virtual ICollection<ProductImages> ProductImages { get; set; } = new List<ProductImages>();

    public virtual ICollection<ProductPriceHistory> ProductPriceHistory { get; set; } = new List<ProductPriceHistory>();
}
