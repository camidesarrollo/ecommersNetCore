using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class Categories
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Image { get; set; }

    public string? BgClass { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; }

    public long? ParentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Categories> InverseParent { get; set; } = new List<Categories>();

    public virtual Categories? Parent { get; set; }

    public virtual ICollection<Products> Products { get; set; } = new List<Products>();
}
