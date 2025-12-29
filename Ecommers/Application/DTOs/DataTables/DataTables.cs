using System.Collections.Generic;

namespace Ecommers.Application.DTOs.DataTables
{
    /// <summary>
    /// Modelo estándar para peticiones de DataTables.js con server-side processing.
    /// </summary>
    public class DataTableRequest<T>
    {
        /// <summary>
        /// Número de draw enviado por DataTables para confirmar respuesta.
        /// CRÍTICO: Debe ser mayor a 0 para que DataTables procese la respuesta.
        /// </summary>
        public required int Draw
        {
            get; set;
        }

        /// <summary>
        /// Inicio del paginado (offset/skip).
        /// </summary>
        public required int Start
        {
            get; set;
        }

        /// <summary>
        /// Tamaño del paginado (rows per page).
        /// </summary>
        public required int Length
        {
            get; set;
        }

        /// <summary>
        /// Columnas enviadas por DataTables.
        /// </summary>
        public required List<DataTableColumn> Columns
        {
            get; set;
        }

        /// <summary>
        /// Filtro general de búsqueda.
        /// </summary>
        public required DataTableSearch Search
        {
            get; set;
        }

        /// <summary>
        /// Configuración de ordenamiento.
        /// </summary>
        public required List<DataTableOrder> Order
        {
            get; set;
        }

        /// <summary>
        /// Datos extras personalizados (opcional).
        /// </summary>
        public required T? AdditionalData { get; set; }
    }

    /// <summary>
    /// Representa una columna enviada desde DataTables.
    /// </summary>
    public class DataTableColumn
    {
        /// <summary>
        /// Nombre del campo de datos.
        /// </summary>
        public required string Data
        {
            get; set;
        }

        /// <summary>
        /// Nombre de la columna.
        /// </summary>
        public required string Name
        {
            get; set;
        }

        /// <summary>
        /// Indica si la columna es buscable.
        /// </summary>
        public required bool Searchable { get; set; }

        /// <summary>
        /// Indica si la columna es ordenable.
        /// </summary>
        public required bool Orderable { get; set; }

        /// <summary>
        /// Búsqueda específica de la columna.
        /// </summary>
        public required DataTableSearch Search
        {
            get; set;
        }
    }

    /// <summary>
    /// Representa un filtro de búsqueda (general o por columna).
    /// </summary>
    public class DataTableSearch
    {
        /// <summary>
        /// Valor de búsqueda.
        /// </summary>
        public required string Value
        {
            get; set;
        }

        /// <summary>
        /// Indica si la búsqueda es regex.
        /// </summary>
        public required bool Regex
        {
            get; set;
        }
    }

    /// <summary>
    /// Representa una instrucción de ordenamiento.
    /// </summary>
    public class DataTableOrder
    {
        /// <summary>
        /// Índice de la columna a ordenar.
        /// </summary>
        public required int Column { get; set; }

        /// <summary>
        /// Dirección del ordenamiento: "asc" o "desc".
        /// </summary>
        public required string Dir
        {
            get; set;
        }
    }

    /// <summary>
    /// Modelo de respuesta para DataTables.js
    /// </summary>
    /// <typeparam name="T">Tipo de entidad de los datos</typeparam>
    public class DataTableResponse<T>
    {
        /// <summary>
        /// Número de draw (debe coincidir con el request).
        /// </summary>
        public required int Draw { get; set; }

        /// <summary>
        /// Total de registros en la base de datos (sin filtrar).
        /// </summary>
        public required int TotalCount { get; set; }

        /// <summary>
        /// Total de registros después de aplicar filtros.
        /// </summary>
        public required int FilteredCount { get; set; }

        /// <summary>
        /// Datos de la página actual.
        /// </summary>
        public required List<T> Data
        {
            get; set;
        }
    }
}