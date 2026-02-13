using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommers.Infrastructure.Web.TagHelpers
{
    [HtmlTargetElement("ui-image")]
    public class ImageTagHelpers : TagHelper
    {
        [HtmlAttributeName("ui-image-model")]
        public dynamic UiImageModel { get; set; } = null!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;

            var images = UiImageModel.Images as IEnumerable<object>;
            var name = UiImageModel.Name;
            var nombreCompuesto = UiImageModel?.NombreCompuesto ?? "";
            var nombreCostumizado = UiImageModel?.NombreCostumizado ?? false;

            var sb = new StringBuilder();
            int index = 0;

            sb.Append($$"""
<div id="{{name}}Container" class="space-y-3 md:space-y-4">
""");

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {

                    sb.Append(RenderImageBlock(image, name, nombreCompuesto, nombreCostumizado, index));
                    index++;
                }
            }

            sb.Append("</div>");

            // 🔹 Mensaje cuando no hay imágenes
            sb.Append($$"""
<div id="no{{name}}Message"
     class="text-center py-8 md:py-12 px-4 text-gray-500 bg-gray-50 rounded-lg border-2 border-dashed border-gray-300 {{(index > 0 ? "hidden" : "")}}">
    <i class="fas fa-image text-4xl md:text-5xl mb-3 md:mb-4 text-gray-400"></i>
    <p class="text-base md:text-lg font-medium mb-1">No hay imágenes agregadas</p>
    <p class="text-xs md:text-sm">Haz clic en "Agregar Imagen" para comenzar</p>
</div>
""");

            output.Content.SetHtmlContent(sb.ToString());
        }

        private string RenderImageBlock(object image, string name, string nombreCompuesto, bool nombreCostumizado,  int i)
        {
            var type = image.GetType();

            if(nombreCostumizado == true)
            {
                name = "";
            }

            var id = type.GetProperty("Id")?.GetValue(image);
            var url = type.GetProperty("Url")?.GetValue(image)?.ToString();
            var alt = type.GetProperty("AltText")?.GetValue(image)?.ToString();
            var order = type.GetProperty("SortOrder")?.GetValue(image);
            var isPrimary = (bool)(type.GetProperty("IsPrimary")?.GetValue(image) ?? false);
            var accionRemover = id?.ToString() != "" ? $"confirmDeleteImageProducto(event, {id}, {i})" : $"removeImageInput(event, {i})";

            return $$"""
<div class="border-2 border-olive-green-300 rounded-lg p-3 md:p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md"
     data-image-index="{{i}}">

    <input type="hidden" name="{{nombreCompuesto}}{{name}}[{{i}}].Id" value="{{id}}" />

    <div class="image-block-layout">

        <!-- Preview -->
        <div class="image-preview-container">
            <div class="image-preview-box">
                {{(string.IsNullOrWhiteSpace(url)
    ? $"""
        <img id="{nombreCompuesto}_preview_{i}" src="" class="w-full h-full object-cover hidden">
        <i id="{nombreCompuesto}_icon_{i}" class="fas fa-image text-3xl md:text-4xl text-gray-400"></i>
      """
    : $"""
        <img id="{nombreCompuesto}_preview_{i}" src="{url}" alt="{alt}" class="w-full h-full object-cover" />
        <i id="{nombreCompuesto}_icon_{i}" class="fas fa-image text-3xl md:text-4xl text-gray-400 hidden"></i>
      """)}}
            </div>
        </div>

        <!-- Controles -->
        <div class="image-controls-container">

            <div>
                <label for="{{nombreCompuesto}}_file_{{i}}" class="block font-semibold mb-2 text-sm md:text-base">
                    Archivo de imagen <span class="text-red-700">*</span>
                </label>

                <input type="file"
                          id="{{nombreCompuesto}}_file_{{i}}"
                       name="{{nombreCompuesto}}{{name}}[{{i}}].ImageFile"
                       accept="image/*"
                       class="image-file-input" />

                <p class="text-xs text-gray-500 mt-2 flex items-center gap-1">
                    <i class="fas fa-info-circle"></i>
                    Formatos: JPG, PNG, WebP
                </p>
            </div>

            <div class="image-order-grid">
                <div>
                    <label for="{{nombreCompuesto}}_sortorder_{{i}}" class="block font-semibold mb-2 text-xs md:text-sm">
                        Orden <span class="text-red-700">*</span>
                    </label>

                    <input type="number"
                            id="{{nombreCompuesto}}_sortorder_{{i}}"
                           name="{{nombreCompuesto}}{{name}}[{{i}}].SortOrder"
                           value="{{order}}"
                           min="0"
                           class="w-full px-3 md:px-4 py-2 md:py-3 text-sm md:text-base border-2 border-gray-300 rounded-lg
                                  focus:border-olive-green-500 outline-none transition" />
                </div>

                <div class="flex items-end">
                    <label for="{{nombreCompuesto}}_primary_{{i}}" class="image-primary-label">
                        <input type="radio"
                                 id="{{nombreCompuesto}}_primary_{{i}}"
                               name="{{nombreCompuesto}}PrimaryImageIndex"
                               value="{{i}}"
                               {{(isPrimary ? "checked" : "")}}
                               onchange="updatePrimaryImage({{i}})"
                               class="w-4 h-4 md:w-5 md:h-5 text-olive-green-600 focus:ring-olive-green-500 cursor-pointer flex-shrink-0" />
                        <span class="text-xs md:text-sm font-semibold text-dark-chocolate whitespace-nowrap">Imagen principal</span>
                    </label>
                </div>
            </div>

            <input type="hidden"
                   name="{{nombreCompuesto}}{{name}}[{{i}}].IsPrimary"
                   id="isPrimary_{{i}}"
                   value="{{isPrimary.ToString().ToLower()}}" />
        </div>

        <button type="button"
                onclick="{{accionRemover}}"
                aria-label="Eliminar imagen del {{nombreCompuesto}}{{name}}[{{i}}]"
                class="image-delete-button"
                title="Eliminar imagen">
            <i class="fas fa-trash-alt text-base md:text-lg"></i>
        </button>
    </div>
</div>
""";
        }
    }
}