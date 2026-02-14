using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_ProductStarMetrics
{
    public long VariantId { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? VariantName { get; set; }

    public string Slug { get; set; } = null!;

    public string SKU { get; set; } = null!;

    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public string StockStatus { get; set; } = null!;

    public long StockQuantity { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Sales_90_Days { get; set; }

    public decimal Revenue_90_Days { get; set; }

    public int Sales_30_Days { get; set; }

    public decimal Revenue_30_Days { get; set; }

    public int TotalSales { get; set; }

    public decimal TotalRevenue { get; set; }

    public int? TotalOrders { get; set; }

    public decimal? TrendPercentage { get; set; }

    public decimal? StarScore { get; set; }

    public string StarLevel { get; set; } = null!;

    public string? Atributos { get; set; }
}
