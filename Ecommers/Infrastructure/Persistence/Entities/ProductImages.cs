using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ProductImages
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Url { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public int Order { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Products Product { get; set; } = null!;
}
