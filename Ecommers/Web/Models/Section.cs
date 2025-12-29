namespace Ecommers.Web.Models
{
    public class Section
    {
        public required string Title { get; set; }
        public required List<MenuItem> Items { get; set; }
    }
}
