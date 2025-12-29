using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class MasterAttributes
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Slug { get; set; } = null!;

    public string DataType { get; set; } = null!;

    public string InputType { get; set; } = null!;

    public string? Unit { get; set; }

    public bool IsRequired { get; set; }

    public bool IsFilterable { get; set; }

    public bool IsVariant { get; set; }

    public string? Category { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<AttributeValues> AttributeValues { get; set; } = new List<AttributeValues>();
}
