using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_ProductFullDetails
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public long? VariantId { get; set; }

    public string? SKU { get; set; }

    public string? VariantName { get; set; }

    public decimal? Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public long? StockQuantity { get; set; }

    public string? StockStatus { get; set; }

    public bool? IsDefault { get; set; }

    public string? MainImageUrl { get; set; }

    public string? VariantImageUrl { get; set; }
}
