using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class Orders
{
    public long Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal Subtotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal ShippingAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal TotalAmount { get; set; }

    public string Currency { get; set; } = null!;

    public string BillingAddress { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public string? PaymentMethod { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime? ShippedAt { get; set; }

    public DateTime? DeliveredAt { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<OrderItems> OrderItems { get; set; } = new List<OrderItems>();

    public virtual AspNetUsers User { get; set; } = null!;
}
