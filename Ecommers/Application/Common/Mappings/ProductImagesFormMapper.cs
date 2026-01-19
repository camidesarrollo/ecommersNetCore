using System.Text.RegularExpressions;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductImagesFormMapper
    {
        public static ProductImagesCreateRequest CrearImagenProductoDesdeForm(
                                                                                IFormCollection form,
                                                                                (int Index, IFormFile File) img,
                                                                                long productId,
                                                                                string nombreProducto)
        {
            var altText = form[$"ProductImages[{img.Index}].AltText"].ToString();
            var sortOrder = int.TryParse(form[$"ProductImages[{img.Index}].SortOrder"], out var order)
                ? order
                : img.Index + 1;
            var isPrimary = form[$"ProductImages[{img.Index}].IsPrimary"] == "true";

            return new ProductImagesCreateRequest
            {
                ProductId = productId,
                AltText = string.IsNullOrWhiteSpace(altText) ? nombreProducto : altText,
                SortOrder = sortOrder,
                IsActive = true,
                IsPrimary = isPrimary
            };
        }

        public static List<(int Index, IFormFile File)> ObtenerImagenesConIndice(IFormFileCollection files)
        {
            return files
                .Where(f => f.Name.StartsWith("ProductImages[") && f.Name.EndsWith(".ImageFile"))
                .Select(f =>
                {
                    var match = Regex.Match(f.Name, @"ProductImages\[(\d+)\]");
                    return (
                        Index: match.Success ? int.Parse(match.Groups[1].Value) : -1,
                        File: f
                    );
                })
                .Where(x => x.Index >= 0)
                .OrderBy(x => x.Index)
                .ToList();
        }

    }
}
