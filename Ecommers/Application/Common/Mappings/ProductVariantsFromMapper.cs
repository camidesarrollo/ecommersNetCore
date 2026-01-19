using System.Text.RegularExpressions;
using Ecommers.Application.DTOs.Requests.ProductVariants;

namespace Ecommers.Application.Common.Mappings
{
    public class ProductVariantsFromMapper
    {
        public static ProductVariantsCreateRequest MapearVariante(IGrouping<int, dynamic> grupo, long productId)
        {
            var variant = new ProductVariantsCreateRequest { ProductId = productId };

            foreach (var field in grupo)
            {
                switch (field.Property)
                {
                    case "SKU":
                        variant.SKU = field.Value;
                        break;

                    case "Price":
                        variant.Price = decimal.TryParse(field.Value, out decimal price) ? price : 0;
                        break;

                    case "CompareAtPrice":
                        variant.CompareAtPrice = decimal.TryParse(field.Value, out decimal compare) ? compare : null;
                        break;

                    case "CostPrice":
                        variant.CostPrice = decimal.TryParse(field.Value, out decimal cost) ? cost : null;
                        break;

                    case "StockQuantity":
                        variant.StockQuantity = long.TryParse(field.Value, out long stock) ? stock : 0;
                        break;

                    case "ManageStock":
                        variant.ManageStock = bool.TryParse(field.Value, out bool manage) && manage;
                        break;

                    case "StockStatus":
                        variant.StockStatus = field.Value;
                        break;

                    case "IsDefault":
                        variant.IsDefault = bool.TryParse(field.Value, out bool isDefault) && isDefault;
                        break;

                    case "IsActive":
                        variant.IsActive = bool.TryParse(field.Value, out bool isActive) && isActive;
                        break;
                }
            }

            return variant;
        }

        public  static List<IGrouping<int, VariantField>> AgruparCamposVariantes(IFormCollection form)
        {
            return form
                .Where(k => k.Key.StartsWith("ProductVariants["))
                .Select(k =>
                {
                    var match = Regex.Match(k.Key, @"ProductVariants\[(\d+)\]\.(.+)");

                    return new VariantField
                    {
                        VariantIndex = match.Success ? int.Parse(match.Groups[1].Value) : -1,
                        Property = match.Success ? match.Groups[2].Value : string.Empty,
                        Value = k.Value.ToString()
                    };
                })
                .Where(x => x.VariantIndex >= 0 && !string.IsNullOrWhiteSpace(x.Value))
                .GroupBy(x => x.VariantIndex)
                .OrderBy(g => g.Key)
                .ToList();
        }
    }
}
