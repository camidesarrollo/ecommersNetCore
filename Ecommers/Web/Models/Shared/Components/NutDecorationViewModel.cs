namespace Ecommers.Web.Models.Shared.Components
{
    public class NutDecorationViewModel
    {
        public int Count { get; set; }
        public required string Size { get; set; }
        public double Opacity { get; set; }
        public required string[] NutTypes { get; set; }
    }
}
