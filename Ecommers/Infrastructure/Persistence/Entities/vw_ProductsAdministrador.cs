using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_ProductsAdministrador
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal? BasePrice { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategorySlug { get; set; } = null!;

    public string? MainImageUrl { get; set; }

    public int? TotalImages { get; set; }

    public int? TotalVariants { get; set; }

    public long TotalStock { get; set; }

    public decimal? MinVariantPrice { get; set; }

    public decimal? MaxVariantPrice { get; set; }

    public string StockStatus { get; set; } = null!;

    public string? MainAttributes { get; set; }

    public string? Brand { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? VariantSKUs { get; set; }

    public string? DefaultSKU { get; set; }

    public int HasSales { get; set; }

    public string Status { get; set; } = null!;

    public string? CreatedAtFormatted { get; set; }

    public string? UpdatedAtFormatted { get; set; }
}
