using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_ProductCatalog
{
    public string typeProduct { get; set; } = null!;

    public string? Name { get; set; }

    public string? Variant { get; set; }

    public string Slug { get; set; } = null!;

    public string SKU { get; set; } = null!;

    public long CategoriaID { get; set; }

    public string Categoria { get; set; } = null!;

    public int IsOnSale { get; set; }

    public bool? IsFeatured { get; set; }

    public string? Atributos { get; set; }

    public bool IsActive { get; set; }

    public string StockStatus { get; set; } = null!;

    public long Id { get; set; }

    public string? Imagen { get; set; }

    public decimal? Price { get; set; }

    public decimal? SalePrice { get; set; }

    public DateTime CreatedAt { get; set; }
}
