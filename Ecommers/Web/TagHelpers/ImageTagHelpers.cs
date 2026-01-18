using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Ecommers.Web.TagHelpers
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

            var sb = new StringBuilder();
            int index = 0;

            sb.Append($$"""
<div id="{{name}}Container" class="space-y-4">
""");

            if (images != null && images.Any())
            {
                foreach (var image in images)
                {
                    sb.Append(RenderImageBlock(image, name, nombreCompuesto, index));
                    index++;
                }
            }

            sb.Append("</div>");

            // 🔹 Mensaje cuando no hay imágenes
            sb.Append($$"""
<div id="no{{name}}Message"
     class="text-center py-12 text-gray-500 bg-gray-50 rounded-lg border-2 border-dashed border-gray-300 {{(index > 0 ? "hidden" : "")}}">
    <i class="fas fa-image text-5xl mb-4 text-gray-400"></i>
    <p class="text-lg font-medium mb-1">No hay imágenes agregadas</p>
    <p class="text-sm">Haz clic en "Agregar Imagen" para comenzar</p>
</div>
""");

            output.Content.SetHtmlContent(sb.ToString());
        }

        private string RenderImageBlock(object image, string name, string nombreCompuesto,  int i)
        {
            var type = image.GetType();

            var id = type.GetProperty("Id")?.GetValue(image);
            var url = type.GetProperty("Url")?.GetValue(image)?.ToString();
            var alt = type.GetProperty("AltText")?.GetValue(image)?.ToString();
            var order = type.GetProperty("Order")?.GetValue(image);
            var isPrimary = (bool)(type.GetProperty("IsPrimary")?.GetValue(image) ?? false);

            return $$"""
<div class="border-2 border-olive-green-300 rounded-lg p-5 bg-white hover:border-olive-green-500 transition-all duration-300 shadow-sm hover:shadow-md"
     data-image-index="{{i}}">

    <input type="hidden" name="{{nombreCompuesto}}{{name}}[{{i}}].Id" value="{{id}}" />

    <div class="flex items-start gap-4">

        <!-- Preview -->
        <div class="flex-shrink-0">
            <div class="w-28 h-28 border-2 border-gray-300 rounded-lg overflow-hidden bg-gray-50 flex items-center justify-center">
                {{(string.IsNullOrWhiteSpace(url)
    ? $"""
        <img id="{nombreCompuesto}_preview_{i}" src="" class="w-full h-full object-cover hidden">
        <i id="{nombreCompuesto}_icon_{i}" class="fas fa-image text-4xl text-gray-400"></i>
      """
    : $"""
        <img id="{nombreCompuesto}_preview_{i}" src="{url}" alt="{alt}" class="w-full h-full object-cover" />
        <i id="{nombreCompuesto}_icon_{i}" class="fas fa-image text-4xl text-gray-400 hidden"></i>
      """)}}
            </div>
        </div>

        <!-- Controles -->
        <div class="flex-1 space-y-4">

            <div>
                <label class="block font-semibold mb-2">
                    Archivo de imagen <span class="text-red-700">*</span>
                </label>

                <input type="file"
                       name="{{nombreCompuesto}}{{name}}[{{i}}].ImageFile"
                       accept="image/*"
                       class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg
                              focus:border-olive-green-500 focus:ring-olive-green-500/20
                              outline-none transition-all duration-300
                              file:mr-4 file:py-2 file:px-4 file:rounded-lg file:border-0
                              file:bg-olive-green-500 file:text-white file:font-semibold
                              hover:file:bg-olive-green-600 cursor-pointer" />

                <p class="text-xs text-gray-500 mt-2 flex items-center gap-1">
                    <i class="fas fa-info-circle"></i>
                    Formatos: JPG, PNG, WebP
                </p>
            </div>

            <div class="grid grid-cols-2 gap-4">
                <div>
                    <label class="block font-semibold mb-2 text-sm">
                        Orden <span class="text-red-700">*</span>
                    </label>

                    <input type="number"
                           name="{{nombreCompuesto}}{{name}}[{{i}}].SortOrder"
                           value="{{order}}"
                           min="0"
                           class="w-full px-4 py-3 border-2 border-gray-300 rounded-lg
                                  focus:border-olive-green-500 outline-none transition" />
                </div>

                <div class="flex items-end">
                    <label class="flex items-center gap-2 cursor-pointer p-3 rounded-lg hover:bg-olive-green-50 transition-colors">
                        <input type="radio"
                               name="{{nombreCompuesto}}PrimaryImageIndex"
                               value="{{i}}"
                               {{(isPrimary ? "checked" : "")}}
                               onchange="updatePrimaryImage({{i}})"
                               class="w-5 h-5 text-olive-green-600 focus:ring-olive-green-500 cursor-pointer" />
                        <span class="text-sm font-semibold text-dark-chocolate">Imagen principal</span>
                    </label>
                </div>
            </div>

            <input type="hidden"
                   name="{{nombreCompuesto}}{{name}}[{{i}}].IsPrimary"
                   id="isPrimary_{{i}}"
                   value="{{isPrimary.ToString().ToLower()}}" />
        </div>

        <button type="button"
                onclick="removeImageInput({{i}})"
                class="flex-shrink-0 text-red-600 hover:text-white hover:bg-red-600 transition-all duration-200 p-3 rounded-lg"
                title="Eliminar imagen">
            <i class="fas fa-trash-alt text-lg"></i>
        </button>
    </div>
</div>
""";
        }
    }
}
