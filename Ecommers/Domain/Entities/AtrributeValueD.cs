using Ecommers.Domain.Common;

namespace Ecommers.Domain.Entities
{
    public class AtrributeValueD : BaseEntity<long>
    {
        public string? ValueString { get; set; }

        public string? ValueText { get; set; }

        public decimal? ValueDecimal { get; set; }

        public int? ValueInt { get; set; }

        public bool? ValueBoolean { get; set; }

        public DateOnly? ValueDate { get; set; }


    }
}
