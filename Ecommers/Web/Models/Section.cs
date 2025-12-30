namespace Ecommers.Web.Models
{
    public class Section
    {
        public string Title { get; set; }
        public List<MenuItem> Items { get; set; } = [];
        public bool IsOpen { get; set; }
    }
}
