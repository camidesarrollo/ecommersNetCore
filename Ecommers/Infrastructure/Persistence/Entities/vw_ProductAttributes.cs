using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class vw_ProductAttributes
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string AttributeName { get; set; } = null!;

    public string AttributeSlug { get; set; } = null!;

    public string AppliesTo { get; set; } = null!;

    public string? AttributeValue { get; set; }

    public string? Unit { get; set; }
}
