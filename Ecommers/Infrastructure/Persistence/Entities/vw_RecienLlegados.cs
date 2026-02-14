using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_RecienLlegados
{
    public long VariantId { get; set; }

    public string SKU { get; set; } = null!;

    public string? VariantName { get; set; }

    public decimal Price { get; set; }

    public decimal? CompareAtPrice { get; set; }

    public long StockQuantity { get; set; }

    public string StockStatus { get; set; } = null!;

    public bool IsDefault { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductSlug { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategorySlug { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? PesoNeto { get; set; }

    public string? Tamano { get; set; }

    public string? FormatoVenta { get; set; }

    public string? Marca { get; set; }

    public int TieneDescuento { get; set; }

    public int? PorcentajeDescuento { get; set; }

    public string DisponibilidadTexto { get; set; } = null!;

    public int? DiasDesdeCreacion { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string? FechaCreacionFormatted { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
