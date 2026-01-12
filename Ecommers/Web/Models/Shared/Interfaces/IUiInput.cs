using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Ecommers.Web.Models.Shared.TagHelpers
{
    public class IUiInput
    {
        // Binding con el modelo (opcional)
        public ModelExpression? AspFor { get; set; }

        // Atributos del componente
        public required string Id { get; set; }
        public required string Label { get; set; }
        public string? Icon { get; set; }
        public string? PrefixIcon { get; set; }
        public string? SuffixIcon { get; set; }
        public string? Placeholder { get; set; }
        public string? Error { get; set; }
        public string? Success { get; set; }
        public string? HelperText { get; set; }
        public string Variant { get; set; } = "default"; // default, filled, outlined
        public string Size { get; set; } = "md"; // sm, md, lg

        public string Type { get; set; } = "text";

        public bool Disabled { get; set; }
        public bool ReadOnly { get; set; }
        public bool Required { get; set; }
        public bool Loading { get; set; }

        public int? MaxLength { get; set; }
        public bool ShowCounter { get; set; }

        public bool ShowPasswordToggle { get; set; } = true;

   
    }
}
