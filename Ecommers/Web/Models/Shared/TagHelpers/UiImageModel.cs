namespace Ecommers.Web.Models.Shared.TagHelpers
{
    public class UiImageModel<T> where T : IUiImage
    {
        public List<T> Images { get; set; } = [];

        public string Name => typeof(T).Name;
    }

}