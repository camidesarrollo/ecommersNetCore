namespace Ecommers.Web.Models.Shared.TagHelpers
{
    public interface IUiImage
    {
        long Id { get; set; }
        string Url { get; set; }
        string? AltText { get; set; }
        bool IsPrimary { get; set; }
        int SortOrder { get; set; }
        bool IsActive { get; set; }
    }
}
