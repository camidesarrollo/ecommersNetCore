namespace Ecommers.Web.Models
{
    public class Section
    {
        public required string Title { get; set; }
        public List<MenuItem> Items { get; set; } = [];
        public bool IsOpen { get; set; }
    }
}
