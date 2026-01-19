using System.Text.RegularExpressions;

namespace Ecommers.Application.Common.Mappings
{
    public partial class ProductVariantImagesFormMapper
    {
        [GeneratedRegex(
            @"ProductVariants\[(\d+)\]\.ProductVariantImages\[(\d+)\]\.ImageFile",
            RegexOptions.Compiled
        )]
        private static partial Regex ProductVariantImageRegex();

        public static List<(int VariantIndex, int ImageIndex, IFormFile File)>
            ObtenerImagenesVariantes(IFormFileCollection files)
        {
            return [.. files
                .Where(f =>
                    f.Name.StartsWith("ProductVariants[") &&
                    f.Name.Contains(".ProductVariantImages[") &&
                    f.Name.EndsWith(".ImageFile"))
                .Select(f =>
                {
                    var match = ProductVariantImageRegex().Match(f.Name);

                    return (
                        VariantIndex: int.Parse(match.Groups[1].Value),
                        ImageIndex: int.Parse(match.Groups[2].Value),
                        File: f
                    );
                })
                .OrderBy(x => x.VariantIndex)
                .ThenBy(x => x.ImageIndex)];
        }
    }
}
