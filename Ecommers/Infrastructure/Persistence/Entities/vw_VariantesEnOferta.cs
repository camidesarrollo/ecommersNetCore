using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_VariantesEnOferta
{
    public long VariantId { get; set; }

    public string SKU { get; set; } = null!;

    public string? VariantName { get; set; }

    public decimal PrecioActual { get; set; }

    public decimal? PrecioOriginal { get; set; }

    public long StockQuantity { get; set; }

    public string StockStatus { get; set; } = null!;

    public bool IsDefault { get; set; }

    public decimal? MontoDescuento { get; set; }

    public decimal? PorcentajeDescuento { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductSlug { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string CategorySlug { get; set; } = null!;

    public string? ProductImageUrl { get; set; }

    public string? VariantImageUrl { get; set; }

    public string? ImageUrl { get; set; }

    public string? PesoNeto { get; set; }

    public string? Tamano { get; set; }

    public string EstadoDisponibilidad { get; set; } = null!;

    public string? Marca { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? FechaCreacion { get; set; }

    public string? FechaActualizacion { get; set; }
}
