using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class Banners
{
    public long Id { get; set; }

    public string Seccion { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string AltText { get; set; } = null!;

    public string Subtitulo { get; set; } = null!;

    public string Titulo { get; set; } = null!;

    public string BotonTexto { get; set; } = null!;

    public string BotonEnlace { get; set; } = null!;

    public bool IsActive { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
