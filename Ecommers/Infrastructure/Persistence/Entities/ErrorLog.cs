using System;
using System.Collections.Generic;

namespace Ecommers.Infrastructure.Persistence.Entities;

public partial class ErrorLog
{
    public int CodigoErrorLog { get; set; }

    public string? Mensaje { get; set; }

    public string? StackTrace { get; set; }

    public string? Metodo { get; set; }

    public string? Usuario { get; set; }

    public string? Ruta { get; set; }

    public string? DatosExtra { get; set; }

    public DateTime CreateAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public DateTime? DeletedAt { get; set; }
}
