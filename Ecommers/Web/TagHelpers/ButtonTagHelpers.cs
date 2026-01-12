using Ecommers.Web.Models.Shared.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommers.Web.TagHelpers
{
    [HtmlTargetElement("ui-button")]
    public class ButtonTagHelpers : TagHelper
    {
        public required IUiButton UiButtonModel { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {


            output.TagName ="Button";

            var disabled = UiButtonModel.Loading || !UiButtonModel.IsFormValid ? "disabled" : "";

            var tipoMap = new Dictionary<string, string>
            {
                {"agregar", "bg-olive-green text-white hover:bg-olive-green-dark focus:ring-olive-green"},
                {"eliminar", "bg-burgundy-red text-white hover:bg-burgundy-red-dark focus:ring-burgundy-red"},
                {"editar", "bg-golden-yellow text-dark-chocolate hover:bg-golden-yellow-dark focus:ring-golden-yellow"},
                {"mas", "bg-mint-green text-dark-chocolate hover:bg-mint-green-dark focus:ring-mint-green"},
                {"menos", "bg-orange-warm text-white hover:bg-orange-warm-dark focus:ring-orange-warm"},
                {"informacion", "bg-white hover:bg-gray-50 border-2 border-yellow-400 text-yellow-600"},
                {"whatsapp", "bg-white hover:bg-gray-50 border-2 border-yellow-400 text-yellow-600"},
                {"default", "bg-gray-light text-gray-dark hover:bg-gray-light-dark focus:ring-gray-light"}
            };

            var tipoClass = tipoMap.TryGetValue(UiButtonModel.Tipo, out string? value) ? value : tipoMap["default"];

            output.Attributes.SetAttribute("class",
                $"block w-full px-4 py-3 rounded-lg font-semibold transition-colors " +
                $"focus:outline-none focus:ring-4 focus:ring-opacity-50 {tipoClass} " +
                $"{(UiButtonModel.Loading ? "opacity-50 cursor-not-allowed" : "")}");

            output.Attributes.SetAttribute("type", UiButtonModel.TipoBoton);

            if (!string.IsNullOrEmpty(UiButtonModel.Accion) && !UiButtonModel.Loading)
                output.Attributes.SetAttribute("onclick", $"{UiButtonModel.Accion}()");

            if (disabled != "")
                output.Attributes.SetAttribute("disabled", "");

            string content = UiButtonModel.Loading
                ? $@"
                <span>
                    <i class='fas fa-spinner fa-spin mr-2'></i>
                    {UiButtonModel.LoadingText}
                </span>"
                : $@"
                <span>
                    {(string.IsNullOrEmpty(UiButtonModel.Icono) ? "" : $"<i class='{UiButtonModel.IconFamily} fa-{UiButtonModel.Icono} mr-2'></i>")}
                    {UiButtonModel.Label}
                </span>";

            output.Content.SetHtmlContent(content);
        }
    }
}
