using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_ProductStarMetrics
{
    public long Id { get; set; }

    public string typeProduct { get; set; } = null!;

    public string? Name { get; set; }

    public string? Variant { get; set; }

    public string Slug { get; set; } = null!;

    public string SKU { get; set; } = null!;

    public long CategoriaID { get; set; }

    public string Categoria { get; set; } = null!;

    public decimal? Price { get; set; }

    public decimal? SalePrice { get; set; }

    public string? Atributos { get; set; }

    public bool IsActive { get; set; }

    public bool? IsFeatured { get; set; }

    public string StockStatus { get; set; } = null!;

    public string? Imagen { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? Sales90Days { get; set; }

    public decimal? Revenue90Days { get; set; }

    public int? Sales30Days { get; set; }

    public decimal? Revenue30Days { get; set; }

    public int? TotalSales { get; set; }

    public decimal? TotalRevenue { get; set; }

    public int? TotalOrders { get; set; }

    public int? TrendPercentage { get; set; }

    public decimal? StarScore { get; set; }

    public string StarLevel { get; set; } = null!;
}
